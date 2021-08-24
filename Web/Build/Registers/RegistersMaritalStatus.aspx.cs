using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class RegistersMaritalStatus : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersMaritalStatus.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvMaritalStatus')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersMaritalStatus.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersMaritalStatus.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuRegistersMaritalStatus.AccessRights = this.ViewState;
            MenuRegistersMaritalStatus.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvMaritalStatus.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        string[] alColumns = { "FLDSHORTNAME", "FLDDESCRIPTION" };
        string[] alCaptions = { "Code", "Description" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersMaritalStatus.MaritalStatusSearch(0, txtSearch.Text, txtDescription.Text, sortexpression, sortdirection,
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=MaritalStatus.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Marital Status</h3></td>");
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

    protected void RegistersMaritalStatus_TabStripCommand(object sender, EventArgs e)
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
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtSearch.Text = "";
                txtDescription.Text = "";
                ViewState["PAGENUMBER"] = 1;
                Rebind();
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
        gvMaritalStatus.SelectedIndexes.Clear();
        gvMaritalStatus.EditIndexes.Clear();
        gvMaritalStatus.DataSource = null;
        gvMaritalStatus.Rebind();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSHORTNAME", "FLDDESCRIPTION" };
        string[] alCaptions = { "Code", "Description" };
        
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersMaritalStatus.MaritalStatusSearch(0, txtSearch.Text, txtDescription.Text, sortexpression, sortdirection,
            int.Parse(ViewState["PAGENUMBER"].ToString()),
            gvMaritalStatus.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvMaritalStatus", "Marital Status", alCaptions, alColumns, ds);

        gvMaritalStatus.DataSource = ds;
        gvMaritalStatus.VirtualItemCount = iRowCount;
        
        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void gvMaritalStatus_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidMaritalStatus(((RadTextBox)e.Item.FindControl("txtMaritalStatusNameAdd")).Text,
                 ((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Text))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                InsertMaritalStatus(
                    ((RadTextBox)e.Item.FindControl("txtMaritalStatusNameAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Text);
                Rebind();
                ((RadTextBox)e.Item.FindControl("txtMaritalStatusNameAdd")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidMaritalStatus(((RadTextBox)e.Item.FindControl("txtMaritalStatusNameEdit")).Text,
                     ((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                UpdateMaritalStatus(
                         Int32.Parse(((RadLabel)e.Item.FindControl("lblMaritalStatusCodeEdit")).Text),
                         ((RadTextBox)e.Item.FindControl("txtMaritalStatusNameEdit")).Text,
                         ((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text
                     );
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["MaritalStatusCode"] = ((RadLabel)e.Item.FindControl("lblMaritalStatusCode")).Text;
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
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMaritalStatus_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
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
    private void InsertMaritalStatus(string MaritalStatusname, string description)
    {
        PhoenixRegistersMaritalStatus.InsertMaritalStatus(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            MaritalStatusname, description);
    }

    private void UpdateMaritalStatus(int MaritalStatuscode, string MaritalStatusname, string description)
    {
        PhoenixRegistersMaritalStatus.UpdateMaritalStatus(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            MaritalStatuscode, MaritalStatusname, description);
        ucStatus.Text = "Marital Status information updated";
    }

    private bool IsValidMaritalStatus(string MaritalStatusname,string description)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvMaritalStatus;

        if (MaritalStatusname.Trim().Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (description.Trim().Equals(""))
            ucError.ErrorMessage = "Description  is required."; 

        return (!ucError.IsError);
    }

    private void DeleteMaritalStatus(int MaritalStatuscode)
    {
        PhoenixRegistersMaritalStatus.DeleteMaritalStatus(0, MaritalStatuscode);
    }
    protected void gvMaritalStatus_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMaritalStatus.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            DeleteMaritalStatus(Int32.Parse(ViewState["MaritalStatusCode"].ToString()));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
