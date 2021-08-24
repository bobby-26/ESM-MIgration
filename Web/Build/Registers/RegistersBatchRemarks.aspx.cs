using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersBatchRemarks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersBatchRemarks.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvBatchRemarks')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");        
        MenuRegistersBatchRemarks.AccessRights = this.ViewState;
        MenuRegistersBatchRemarks.MenuList = toolbar.Show();
        toolbar = new PhoenixToolbar();
        MenuTitle.AccessRights = this.ViewState;
        MenuTitle.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            gvBatchRemarks.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;


        string[] alColumns = { "FLDDESCRIPTION" };
        string[] alCaptions = { "Description" };
        string sortexpression;
        int sortdirection;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        else
            sortdirection = 0;

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixRegistersBatchRemarks.BatchRemarksList(PhoenixSecurityContext.CurrentSecurityContext.UserCode, sortexpression, sortdirection
             , (int)ViewState["PAGENUMBER"], gvBatchRemarks.PageSize, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Batch Remarks.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Batch Remarks</h3></td>");
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

    protected void RegistersBatchRemarks_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvBatchRemarks.Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDDESCRIPTION" };
        string[] alCaptions = { "Description" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int sortdirection;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        else
            sortdirection = 0;

        DataSet ds = PhoenixRegistersBatchRemarks.BatchRemarksList(PhoenixSecurityContext.CurrentSecurityContext.UserCode, sortexpression, sortdirection
            , (int)ViewState["PAGENUMBER"], gvBatchRemarks.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvBatchRemarks", "Batch Remarks", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvBatchRemarks.DataSource = ds;
            gvBatchRemarks.VirtualItemCount = iRowCount;
        }
        else
        {
            gvBatchRemarks.DataSource = "";
        }
    }
    private void InsertBatchDescription(string description)
    {
        if (!IsValidDescription(description))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersBatchRemarks.BatchRemaksAdd(PhoenixSecurityContext.CurrentSecurityContext.UserCode, description);
        
    }
    private void UpdateBatchRemarks(int batchremarksid, string description)
    {
        if (!IsValidDescription(description))
        {
            ucError.Visible = true;
            return;
        }

        PhoenixRegistersBatchRemarks.BatchRemarksUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, batchremarksid, description);
        
        ucStatus.Text = "Batch Remarks information updated";
    }
    private void DeleteBatchRemarks(int batchremarksid)
    {
        PhoenixRegistersBatchRemarks.BatchRemarksDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, batchremarksid);
        
    }
    private bool IsValidDescription(string description)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        
        if (description.Trim().Equals(""))
            ucError.ErrorMessage = "Description is required.";

        return (!ucError.IsError);
    }
 

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvBatchRemarks_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvBatchRemarks.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvBatchRemarks_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
       
        if (e.CommandName.ToUpper().Equals("ADD"))
        {
            InsertBatchDescription(((RadTextBox)e.Item.FindControl("txtDescriptionIdAdd")).Text);
            BindData();
            gvBatchRemarks.Rebind();
        }
        else if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            DeleteBatchRemarks(Int32.Parse(((RadLabel)e.Item.FindControl("lblDescriptionId")).Text));
            BindData();
            gvBatchRemarks.Rebind();
        }
        else if (e.CommandName.ToUpper().Equals("UPDATE"))
        {
            UpdateBatchRemarks(
               Int16.Parse(((RadLabel)e.Item.FindControl("lblDescriptionIdEdit")).Text),
               ((RadTextBox)e.Item.FindControl("txtDescriptionIdEdit")).Text);           
            BindData();
            gvBatchRemarks.Rebind();
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvBatchRemarks_ItemDataBound(object sender, GridItemEventArgs e)
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
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

        }
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
}
