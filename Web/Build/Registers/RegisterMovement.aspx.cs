using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using Telerik.Web.UI;

public partial class RegisterMovement : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegisterMovement.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvMovement')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegisterMovement.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegisterMovement.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuRegistersMovement.AccessRights = this.ViewState;
            MenuRegistersMovement.MenuList = toolbar.Show();


            if (!IsPostBack)
            {
                chkActive.Checked = true;
                chkpayable.Checked = true;
                ucGlobalWageComponentSearch.SelectedComponent = "";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvMovement.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void RegistersMovement_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvMovement.CurrentPageIndex = 0;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ucGlobalWageComponentSearch.SelectedComponent = "";
                txtCode.Text = "";
                txtName.Text = "";
                chkActive.Checked = true;
                chkpayable.Checked = true;
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
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int active;
        int payable;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDCODE", "FLDNAME", "FLDACTIVE", "FLDPAYABLE", "FLDWAGECOMPONENTNAME", "FLDPERCENTAGE", "FLDFORMULA" };
        string[] alCaptions = { "Code", "Name", "Active Y/N", "Payable Y/N", "Wage Component", "Percentage", "Formula" };
        string sortexpression;
        int? sortdirection = null;
        if (chkActive.Checked == true)
        {
            active = 1;
        }
        else
        {
            active = 0;
        }
        if (chkpayable.Checked == true)
        {
            payable = 1;
        }
        else
        {
            payable = 0;
        }

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegisterMovement.Movementsearch(txtCode.Text, txtName.Text, active, payable, General.GetNullableGuid(ucGlobalWageComponentSearch.SelectedComponent),
              sortexpression, sortdirection,
             1,
             gvMovement.PageSize,
             ref iRowCount,
             ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Movement.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Airport</h3></td>");
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
    protected void gvMovement_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidAirport(((RadTextBox)e.Item.FindControl("txtAdd")).Text,
                   ((RadTextBox)e.Item.FindControl("txtNameAdd")).Text,
                   ((UserControlGlobalWageComponent)e.Item.FindControl("ucGlobalWageComponent")).SelectedComponent))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegisterMovement.MovementInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    ((RadTextBox)e.Item.FindControl("txtAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtNameAdd")).Text,
                    (((RadCheckBox)e.Item.FindControl("chkActiveYNAdd")).Checked.Equals(true)) ? 1 : 0,
                    (((RadCheckBox)e.Item.FindControl("chkPayableYNAdd")).Checked.Equals(true)) ? 1 : 0,
                    General.GetNullableGuid(((UserControlGlobalWageComponent)e.Item.FindControl("ucGlobalWageComponent")).SelectedComponent),
                    ((RadTextBox)e.Item.FindControl("txtformulaAdd")).Text,
                    General.GetNullableDecimal(((RadTextBox)e.Item.FindControl("txtpercentageAdd")).Text));
                Rebind();
                ucStatus.Show("Movement is added");
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["MovementID"] = ((RadLabel)e.Item.FindControl("lblmovementid")).Text;
                //PhoenixRegisterMovement.MovementDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                //    int.Parse(((RadLabel)e.Item.FindControl("lblmovementid")).Text));
              RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
               //Rebind();
              //ucStatus.Show("Movement is deleted");
                return;

            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixRegisterMovement.MovementUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                   int.Parse(((RadLabel)e.Item.FindControl("lblEdit")).Text),
                    ((RadTextBox)e.Item.FindControl("txtcodeEdit")).Text,
                    ((RadTextBox)e.Item.FindControl("txtNameEdit")).Text,
                    (((RadCheckBox)e.Item.FindControl("chkActiveYNEdit")).Checked.Equals(true)) ? 1 : 0,
                    (((RadCheckBox)e.Item.FindControl("chkPayableYNEdit")).Checked.Equals(true)) ? 1 : 0,
                    General.GetNullableGuid(((UserControlGlobalWageComponent)e.Item.FindControl("ucGlobalWageComponentEdit")).SelectedComponent),
                    ((RadTextBox)e.Item.FindControl("txtformulaEdit")).Text,
                    General.GetNullableDecimal(((RadTextBox)e.Item.FindControl("txtpercentageEdit")).Text));
                Rebind();
                ucStatus.Show("Updated sucessfully");
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
    protected void Rebind()
    {
        gvMovement.SelectedIndexes.Clear();
        gvMovement.EditIndexes.Clear();
        //gvMovement.DataSource = null;
        gvMovement.Rebind();

    }
    private void DeleteMovement(int movementid)
    {
        PhoenixRegisterMovement.MovementDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, movementid);

    }
    private bool IsValidAirport(string code, string name, string componentid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvMovement;
        if (code.Trim().Equals(""))
            ucError.ErrorMessage = "Code is required.";
            //ucStatus.Show("Code is required.");

        if (name.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";
        //ucStatus.Show("Name is required.");
        if ( General.GetNullableGuid(componentid)== null)
           ucError.ErrorMessage = "Component is required.";
        //ucStatus.Show("Component is required");

        return (!ucError.IsError);
    }
    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            DeleteMovement(Int32.Parse(ViewState["MovementID"].ToString()));
            Rebind();
            ucStatus.Show("Deleted Successfully");

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMovement_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
               // ucStatus.Show("DELETE");
                //db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                //if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
        }
        if (e.Item.IsInEditMode)
        {
            string code = ((RadTextBox)e.Item.FindControl("txtcodeEdit")).Text;
            string name = ((RadTextBox)e.Item.FindControl("txtNameEdit")).Text;
            UserControlGlobalWageComponent ddlwagecomponent = (UserControlGlobalWageComponent)e.Item.FindControl("ucGlobalWageComponentEdit");
           // string wagecomponentid = ((RadLabel)e.Item.FindControl("lblwagecomponentid")).Text;
            string formulaedit = ((RadTextBox)e.Item.FindControl("txtformulaEdit")).Text;
            string percentageid = ((RadTextBox)e.Item.FindControl("txtpercentageEdit")).Text;
            string movementid = ((RadLabel)e.Item.FindControl("lblcomponentId")).Text;
            if (movementid != null)
            {
                ddlwagecomponent.SelectedComponent = movementid;
            }

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
    protected void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int active;
        int payable;
        string[] alColumns = { "FLDCODE", "FLDNAME", "FLDACTIVE", "FLDPAYABLE", "FLDWAGECOMPONENTNAME", "FLDPERCENTAGE", "FLDFORMULA" };
        string[] alCaptions = { "Code", "Name", "Active Y/N", "Payable Y/N", "Wage Component", "Percentage","Formula" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (chkActive.Checked == true)
        {
            active = 1;
        }
        else
        {
            active = 0;
        }
        if (chkpayable.Checked == true)
        {
            payable = 1;
        }
        else
        {
            payable = 0;
        }

        DataSet ds = PhoenixRegisterMovement.Movementsearch(txtCode.Text, txtName.Text, active, payable, General.GetNullableGuid(ucGlobalWageComponentSearch.SelectedComponent),
             sortexpression, sortdirection,
            gvMovement.CurrentPageIndex + 1,
            gvMovement.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvMovement", "Movement", alCaptions, alColumns, ds);

        gvMovement.DataSource = ds;
        gvMovement.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void gvMovement_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void gvMovement_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            //ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMovement.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}