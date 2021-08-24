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
public partial class RegistersCrewCostEvaluationSection : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersCrewCostEvaluationSection.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvSection')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersCrewCostEvaluationSection.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersCrewCostEvaluationSection.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");

            MenuSection.AccessRights = this.ViewState;
            MenuSection.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                gvSection.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuSection_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvSection.CurrentPageIndex = 0;
                BindData();
                gvSection.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtSectionName.Text = "";
                ddlActive.SelectedValue = "";
                ddlActive.Text = "";
                ddlSectionType.SelectedValue = "";
                ddlSectionType.SelectedSectionTypeID = string.Empty;

                ViewState["PAGENUMBER"] = 1;
                gvSection.CurrentPageIndex = 0;
                BindData();
                gvSection.Rebind();
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

        string[] alColumns = { "FLDSECTIONTYPENAME", "FLDSECTIONNAME" };
        string[] alCaptions = { "Section Type", "Section" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersCrewCostEvaluationSection.CrewCostEvaluationSectionSearch(General.GetNullableInteger(ddlSectionType.SelectedSectionTypeID.ToString()), General.GetNullableString(txtSectionName.Text)
                    , General.GetNullableInteger(ddlActive.SelectedValue)
                    , sortexpression, sortdirection,
                    (int)ViewState["PAGENUMBER"],
                    gvSection.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

        General.SetPrintOptions("gvSection", "Section", alCaptions, alColumns, ds);

        gvSection.DataSource = ds;
        gvSection.VirtualItemCount = iRowCount;

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
            string[] alColumns = { "FLDSECTIONTYPENAME", "FLDSECTIONNAME" };
            string[] alCaptions = { "Section Type", "Section" };

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            ds = PhoenixRegistersCrewCostEvaluationSection.CrewCostEvaluationSectionSearch(General.GetNullableInteger(ddlSectionType.SelectedSectionTypeID.ToString()), General.GetNullableString(txtSectionName.Text)
                     , General.GetNullableInteger(ddlActive.SelectedValue)
                     , sortexpression, sortdirection,
                     (int)ViewState["PAGENUMBER"],
                     gvSection.PageSize,
                     ref iRowCount,
                     ref iTotalPageCount);

            if (ds.Tables.Count > 0)
                General.ShowExcel("Section", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void InsertSection(int? sectiontypeid, string name, string activeyn)
    {
        if (!IsValidSection(sectiontypeid, name))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersCrewCostEvaluationSection.InsertCrewCostEvaluationSection(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , sectiontypeid
            , name
            , int.Parse(activeyn));
    }

    private void UpdateSection(int? SectionTypeId, Guid SectionId, string name, string activeyn)
    {
        PhoenixRegistersCrewCostEvaluationSection.UpdateCrewCostEvaluationSection(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , SectionTypeId
            , SectionId, name, int.Parse(activeyn));
    }

    private void DeleteSection(Guid Sectionid)
    {
        PhoenixRegistersCrewCostEvaluationSection.DeleteCrewCostEvaluationSection(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, Sectionid);
    }

    private bool IsValidSection(int? sectiontypeid, string name)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (sectiontypeid == null)
            ucError.ErrorMessage = "Section Type is required.";

        if (name == null || name.ToString().Trim().Equals(""))
            ucError.ErrorMessage = "Section Name is required.";

        return (!ucError.IsError);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvSection_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSection.CurrentPageIndex + 1;

        BindData();
    }

    protected void gvSection_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                UserControlCrewCostEvaluationSectionType sectiontypeadd = (UserControlCrewCostEvaluationSectionType)e.Item.FindControl("ucSectionTypeAdd");
                InsertSection(
                    General.GetNullableInteger(sectiontypeadd.SelectedSectionTypeID.ToString()),
                    ((RadTextBox)e.Item.FindControl("txtSectionNameAdd")).Text,
                    ((RadCheckBox)e.Item.FindControl("chkActiveYNAdd")).Checked == true ? "1" : "0"
                );

                BindData();
                gvSection.Rebind();
                ((RadTextBox)e.Item.FindControl("txtSectionNameAdd")).Focus();
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

    protected void gvSection_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            DeleteSection(new Guid(((RadLabel)e.Item.FindControl("lblSectionId")).Text));

            BindData();
            gvSection.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSection_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            UserControlCrewCostEvaluationSectionType sectiontypeid = (UserControlCrewCostEvaluationSectionType)e.Item.FindControl("ucSectionTypeEdit");
            string sectionname = ((RadTextBox)e.Item.FindControl("txtSectionNameEdit")).Text;
            string activeyn = ((RadCheckBox)e.Item.FindControl("chkActiveYNEdit")).Checked == true ? "1" : "0";

            if (!IsValidSection(General.GetNullableInteger(sectiontypeid.SelectedSectionTypeID), General.GetNullableString(sectionname)))
            {
                ucError.Visible = true;
                return;
            }

            UpdateSection(
                 General.GetNullableInteger(sectiontypeid.SelectedSectionTypeID.ToString()),
                 new Guid(((RadLabel)e.Item.FindControl("lblSectionIdEdit")).Text),
                ((RadTextBox)e.Item.FindControl("txtSectionNameEdit")).Text,
                ((RadCheckBox)e.Item.FindControl("chkActiveYNEdit")).Checked == true ? "1" : "0"
             );

            BindData();
            gvSection.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvSection_ItemDataBound1(object sender, GridItemEventArgs e)
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
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            UserControlCrewCostEvaluationSectionType sectiontypeedit = (UserControlCrewCostEvaluationSectionType)e.Item.FindControl("ucSectionTypeEdit");
            if (sectiontypeedit != null) sectiontypeedit.SelectedSectionTypeID = drv["FLDSECTIONTYPEID"].ToString();

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
