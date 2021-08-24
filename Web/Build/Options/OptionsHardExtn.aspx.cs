using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.Profile;
using Telerik.Web.UI;

public partial class OptionsHardExtn : PhoenixBasePage
{
  protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddFontAwesomeButton("../Options/OptionsHardExtn.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvHard')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Options/OptionsHardExtn.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        
        MenuPhoenixHard.MenuList = toolbar.Show();
        //MenuPhoenixHard.SetTrigger(pnlHardEntry);   
        
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("List", "BACK",ToolBarDirection.Right);
        MenuHardExtn.AccessRights = this.ViewState;
        MenuHardExtn.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {           
            ddlHardType.SelectedIndex = -1;
            ddlHardType.DataSource = PhoenixRegistersHardExtn.ListHardTypeExtn(1);
            ddlHardType.DataBind();
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvHard.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
       }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void Rebind()
    {
        gvHard.SelectedIndexes.Clear();
        gvHard.EditIndexes.Clear();
        gvHard.DataSource = null;
        gvHard.Rebind();
    }
    protected void MenuHardExtn_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("BACK"))
            {
                    Response.Redirect("../Options/OptionsHard.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDSHORTNAME", "FLDHARDNAME", };
        string[] alCaptions = {"Code","Name"};
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
      
        ds = PhoenixRegistersHardExtn.HardSearchExtn(0, ddlHardType.SelectedValue, "", "", sortexpression, sortdirection,
             Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvHard.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=Hard.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Hard Register</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
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

    //protected void ddlHardType_TextChanged(object sender, EventArgs e)
    //{
    //    BindData();
    //}

    protected void PhoenixHard_TabStripCommand(object sender, EventArgs e)
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
        string[] alColumns = { "FLDSHORTNAME", "FLDHARDNAME", };
        string[] alCaptions = { "Code", "Name" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
       

        DataSet ds = PhoenixRegistersHardExtn.HardSearchExtn(0, ddlHardType.SelectedValue, "", "", sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvHard.PageSize,
            ref iRowCount,
            ref iTotalPageCount);
        General.SetPrintOptions("gvHard", "Registers", alCaptions, alColumns, ds);
        gvHard.DataSource = ds;
        gvHard.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void gvHard_Sorting(object sender, GridViewSortEventArgs se)
    {
      
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
  
   
    private void InsertHard(string hardtypecode,string Hardname,string Shortname)
    {
        if (!IsValidHard(Shortname,Hardname))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersHardExtn.InsertHardExtn(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            hardtypecode, Hardname, Shortname);
    }

    private void UpdateHard(string Hardtypecode,int Hardcode, string Hardname, string shortname)
    {
        if (!IsValidHard(shortname, Hardname))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersHardExtn.UpdateHardExtn(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            Hardtypecode, Hardcode, Hardname, shortname);
        ucStatus.Text = "System Parameters updated";
    }

    private bool IsValidHard(string Shortname,string Hardname)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Shortname.Trim().Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (Hardname.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";

        
        return (!ucError.IsError);
    }

    private void DeleteHard(int Hardcode,int Hardtypecode)
    {
        PhoenixRegistersHardExtn.DeleteHardExtn(PhoenixSecurityContext.CurrentSecurityContext.UserCode,Hardtypecode, Hardcode);
    }

   

    protected void gvHard_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                InsertHard(
                    ddlHardType.SelectedValue,
                    ((RadTextBox)e.Item.FindControl("txtHardNameAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtShortNameAdd")).Text
                );

                ((RadTextBox)e.Item.FindControl("txtHardNameAdd")).Focus();
                BindData();
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                UpdateHard(
                    ddlHardType.SelectedValue,
                    Int32.Parse(((RadLabel)e.Item.FindControl("lblHardCodeEdit")).Text),
                     ((RadTextBox)e.Item.FindControl("txtHardNameEdit")).Text,
                     ((RadTextBox)e.Item.FindControl("txtShortNameEdit")).Text
                 );
                BindData();
                Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteHard(Int32.Parse(((RadLabel)e.Item.FindControl("lblHardCode")).Text), Int32.Parse(((RadLabel)e.Item.FindControl("lblHardTypeCode")).Text));
                BindData();
                Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvHard_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

        }
        if (e.Item is GridDataItem)
        {
            LinkButton cmdAdd = (LinkButton)e.Item.FindControl("cmdAdd");
            if (cmdAdd != null)
                cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);
        }
    }

    protected void gvHard_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvHard.CurrentPageIndex + 1;
        BindData();
    }
}
