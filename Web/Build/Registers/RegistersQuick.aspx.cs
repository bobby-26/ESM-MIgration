using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class RegistersQuick : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
			toolbar.AddFontAwesomeButton("../Registers/RegistersQuick.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvQuick')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuRegistersQuick.AccessRights = this.ViewState;
            MenuRegistersQuick.MenuList = toolbar.Show();
            
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["QuickCodeType"] = null;
                gvQuick.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                string module = Request.QueryString["module"];
                ucQuickType.QuickTypeGroup = module;
                if (Request.QueryString["quickcodetype"] != null)
                {
                    ViewState["QuickCodeType"] = Request.QueryString["quickcodetype"].ToString();
                    ucQuickType.SelectedQuickType = ViewState["QuickCodeType"].ToString();
                }
                if (Request.QueryString["quickcodetype"] != null)
                {
                    ucQuickType.SelectedQuickType = ViewState["QuickCodeType"].ToString();
                    ucQuickType.QuickTypeShowYesNo = "0";
                    CaptionChange(module, ViewState["QuickCodeType"].ToString());
                    ucQuickType.Visible = false;
                    lblRegister.Visible = false;
                }
                else
                {
                    ucQuickType.bind();
                }                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindFirstValue()
    {
        string quicktype = null;
        if (ViewState["QuickCodeType"] == null || ViewState["QuickCodeType"].ToString() == "")
        {
            if (ucQuickType.SelectedQuickType == "")
            {
                ucQuickType.QuickTypeShowYesNo = "1";
                string yesno = ucQuickType.QuickTypeShowYesNo;
                DataSet ds1 = PhoenixRegistersQuick.ListQuickType(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    ucQuickType.QuickTypeGroup, Convert.ToInt32(yesno));
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    DataRow drActivity = ds1.Tables[0].Rows[0];
                    quicktype = drActivity["FLDQUICKTYPECODE"].ToString();
                    ucQuickType.SelectedQuickType = drActivity["FLDQUICKTYPECODE"].ToString();
                    ucQuickType.bind();
                    ViewState["QuickCodeType"] = drActivity["FLDQUICKTYPECODE"].ToString();
                    ViewState["QuickCodeTypeName"] = drActivity["FLDQUICKTYPENAME"].ToString();
                }
            }
        }
    }
   
    public void CaptionChange(string module,string quicktypecode)
    {
        ucQuickType.Enabled = "false";
        DataSet dsedit = PhoenixRegistersQuick.ListQuickTypeEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode, module, 
                                                        Convert.ToInt32(quicktypecode));

        if (dsedit.Tables.Count > 0)
        {
            DataRow drquick = dsedit.Tables[0].Rows[0];
            //ucTitle.Text = General.GetMixedCase(drquick["FLDQUICKTYPENAME"].ToString());
            if (string.IsNullOrEmpty(module))
            {
                ucQuickType.QuickTypeGroup = drquick["FLDQUICKTYPENAME"].ToString();
                ViewState["MODULENAME"] = drquick["FLDQUICKTYPENAME"].ToString();
            }
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
		string[] alCaptions = new string[2];
        string[] alColumns = { "FLDSHORTNAME","FLDQUICKNAME"};
		
        if (ViewState["QuickCodeType"].ToString() == "65")
		{
			alCaptions[0] =  "Approval Code";
			alCaptions[1] =  "Approval Authority";
		}		
		else
		{
			alCaptions[0] = "Code";
			alCaptions[1] = "Name";
		}
        string sortexpression;
        int? sortdirection = null;
      
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCommonRegisters.QuickSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    ViewState["QuickCodeType"].ToString(), sortexpression, sortdirection,
                    gvQuick.CurrentPageIndex + 1,
                    gvQuick.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

        if(ds.Tables[0].Rows.Count>0)
        {
            DataRow dr=ds.Tables[0].Rows[0];
            ViewState["QUICKTYPENAME"]=Convert.ToString(dr["FLDQUICKTYPENAME"]);
        }

        if (ViewState["MODULENAME"] != null)
        {
            Response.AddHeader("Content-Disposition", "attachment; filename=\""+Convert.ToString(ViewState["QUICKTYPENAME"])+".xls\"");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='https://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>" + ViewState["MODULENAME"].ToString() + "</h3></td>");
            Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
         
        }
        else
        {
            Response.AddHeader("Content-Disposition", "attachment; filename=\"" + Convert.ToString(ViewState["QUICKTYPENAME"]) + ".xls\"");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>" + Convert.ToString(ViewState["QUICKTYPENAME"]) + "</h3></td>");
            Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
           
        }
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();   
  
    }

    protected void RegistersQuick_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {        
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSHORTNAME", "FLDQUICKNAME" };
        string[] alCaptions = { "Code", "Name" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        DataSet ds = new DataSet();
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixCommonRegisters.QuickSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                ViewState["QuickCodeType"].ToString(), sortexpression, sortdirection,
                 (int)ViewState["PAGENUMBER"],
                 gvQuick.PageSize,
                 ref iRowCount,
                 ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr=ds.Tables[0].Rows[0];
            ViewState["QUICKTYPENAME"]=Convert.ToString(dr["FLDQUICKTYPENAME"]);
        }

        General.SetPrintOptions("gvQuick",  Convert.ToString(ViewState["QUICKTYPENAME"]), alCaptions, alColumns, ds);

        gvQuick.DataSource = ds;
        gvQuick.VirtualItemCount = iRowCount;
    }
    protected void gvQuick_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidQuick(((RadTextBox)e.Item.FindControl("txtQuickNameAdd")).Text))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }

                string Quicktypecode = (ViewState["QuickCodeType"].ToString() == "" || ViewState["QuickCodeType"] == null) ? ucQuickType.SelectedQuickType : ViewState["QuickCodeType"].ToString();
                string Quickname = ((RadTextBox)e.Item.FindControl("txtQuickNameAdd")).Text;
                string Shortname = ((RadTextBox)e.Item.FindControl("txtShortNameAdd")).Text;

                PhoenixRegistersQuick.InsertQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Int32.Parse(Quicktypecode), Quickname, Shortname);
                ucStatus.Text = "Information added";
                Rebind();
            }

            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string QuickCode = ((RadLabel)e.Item.FindControl("lblQuickCodeEdit")).Text;
                string Quicktypecode = (ViewState["QuickCodeType"].ToString() == "" || ViewState["QuickCodeType"] == null) ? ucQuickType.SelectedQuickType : ViewState["QuickCodeType"].ToString();
                int Quickcode = Int32.Parse(((RadLabel)e.Item.FindControl("lblQuickCodeEdit")).Text);
                string Quickname = ((RadTextBox)e.Item.FindControl("txtQuickNameEdit")).Text;
                string shortname = ((RadTextBox)e.Item.FindControl("txtShortNameEdit")).Text;

                if (!IsValidQuick(Quickname))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersQuick.UpdateQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Int32.Parse(Quicktypecode), Quickcode, Quickname, shortname);
                ucStatus.Text = "Information updated";
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["QuickCode"] = ((RadLabel)e.Item.FindControl("lblQuickCode")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
                return;
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            e.Canceled = true;
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvQuick.SelectedIndexes.Clear();
        gvQuick.EditIndexes.Clear();
        gvQuick.DataSource = null;
        gvQuick.Rebind();
    }
    protected void gvQuick_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

        LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }
    private bool IsValidQuick(string Quickname)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvQuick;

		if (Quickname.Trim().Equals(""))
		{

            if (ViewState["QuickCodeType"] != null && ViewState["QuickCodeType"].ToString() != "")
			{
                if (ViewState["QuickCodeType"].ToString() == "64")
					ucError.ErrorMessage = "Cancel Reason is required.";
                if (ViewState["QuickCodeType"].ToString() == "65")
					ucError.ErrorMessage = "Approval Status is required.";
                else
                    ucError.ErrorMessage = "Name is required.";
			}
			else
			{
				ucError.ErrorMessage = "Name is required.";
			}
		}
        return (!ucError.IsError);
    }
    protected void gvQuick_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvQuick.CurrentPageIndex + 1;
            BindFirstValue();
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        ViewState["QuickCodeType"] = ucQuickType.SelectedQuickType;
        // CaptionChange(Request.QueryString["module"].ToString(), ViewState["QuickCodeType"].ToString());
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }   
    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixRegistersQuick.DeleteQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Int32.Parse(ViewState["QuickCode"].ToString()));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvQuick_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
}
