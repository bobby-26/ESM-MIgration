using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class RegistersCrewCostEvaluationSectionType : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersCrewCostEvaluationSectionType.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvSectionType')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersCrewCostEvaluationSectionType.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersCrewCostEvaluationSectionType.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");

            MenuSectionType.AccessRights = this.ViewState;
            MenuSectionType.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                gvSectionType.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuSectionType_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvSectionType.CurrentPageIndex = 0;
                BindData();
                gvSectionType.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtSectionTypeName.Text = "";
                ddlActive.SelectedValue = "";
                ddlActive.Text = "";

                ViewState["PAGENUMBER"] = 1;
                gvSectionType.CurrentPageIndex = 0;
                BindData();
                gvSectionType.Rebind();
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

        string[] alColumns = { "FLDSECTIONTYPENAME" };
        string[] alCaptions = { "Section Type" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersCrewCostEvaluationSectionType.CrewCostEvaluationSectionTypeSearch(General.GetNullableString(txtSectionTypeName.Text.Trim())
                    , General.GetNullableInteger(ddlActive.SelectedValue)
                    , sortexpression, sortdirection,
                    (int)ViewState["PAGENUMBER"],
                    gvSectionType.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);


        General.SetPrintOptions("gvSectionType", "Section Type", alCaptions, alColumns, ds);

        gvSectionType.DataSource = ds;
        gvSectionType.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();
            string[] alColumns = { "FLDSECTIONTYPENAME" };
            string[] alCaptions = { "Section Type" };

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


            ds = PhoenixRegistersCrewCostEvaluationSectionType.CrewCostEvaluationSectionTypeSearch(General.GetNullableString(txtSectionTypeName.Text.Trim())
                , General.GetNullableInteger(ddlActive.SelectedValue)
                , sortexpression, sortdirection,
                (int)ViewState["PAGENUMBER"],
                gvSectionType.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            if (ds.Tables.Count > 0)
                General.ShowExcel("Section Type", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvSectionType_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSectionType.CurrentPageIndex + 1;

        BindData();
    }

    protected void gvSectionType_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                InsertSectionType(
                    ((RadTextBox)e.Item.FindControl("txtSectionTypeNameAdd")).Text
                    , ((RadCheckBox)e.Item.FindControl("chkActiveYNAdd")).Checked == true ? "1" : "0"
                );
                BindData();
                gvSectionType.Rebind();

                ((RadTextBox)e.Item.FindControl("txtSectionTypeNameAdd")).Focus();
            }
            if (e.CommandName == "Page")
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

    private void InsertSectionType(string name, string activeyn)
    {
        if (!IsValidSectionType(name))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersCrewCostEvaluationSectionType.InsertCrewCostEvaluationSectionType(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , name
            , int.Parse(activeyn));
    }


    private bool IsValidSectionType(string name)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (name == null || name.ToString().Trim() == "")
            ucError.ErrorMessage = "Section Type is required.";

        return (!ucError.IsError);
    }

    protected void gvSectionType_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
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

    protected void gvSectionType_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            PhoenixRegistersCrewCostEvaluationSectionType.DeleteCrewCostEvaluationSectionType(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblSectionTypeId")).Text));

            BindData();
            gvSectionType.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSectionType_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string sectiontypename = ((RadTextBox)e.Item.FindControl("txtSectionTypeNameEdit")).Text;
            string activeyn = ((RadCheckBox)e.Item.FindControl("chkActiveYNEdit")).Checked == true ? "1" : "0";

            if (!IsValidSectionType(General.GetNullableString(sectiontypename)))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixRegistersCrewCostEvaluationSectionType.UpdateCrewCostEvaluationSectionType(
                                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                               , General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblSectionTypeIdEdit")).Text)
                                               , General.GetNullableString(sectiontypename)
                                               , int.Parse(activeyn));

            BindData();
            gvSectionType.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
