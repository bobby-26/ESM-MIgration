using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class RegistersVesselSurveyType : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersVesselSurveyType.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvSurveyType')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuRegistersSurveyType.AccessRights = this.ViewState;
            MenuRegistersSurveyType.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                gvSurveyType.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
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

        string[] alColumns = { "FLDSHORTCODE", "FLDSURVEYTYPENAME", "FLDACTIVEYNSTATUS", "FLDFREQUENCY", "FLDWINDOWPERIODBEFORE", "FLDWINDOWPERIODAFTER" };
        string[] alCaptions = { "Code", "Name", "Active Y/N", "Frequency(M)", "Window Period Before(M)", "Window Period After(M)" };


        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersVesselSurveyFrequency.SurveySearch(
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        General.ShowExcel("Survey Type", ds.Tables[0], alColumns, alCaptions, null, null);
    }

    protected void MenuRegistersSurveyType_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

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

        string[] alColumns = { "FLDSHORTCODE", "FLDSURVEYTYPENAME", "FLDACTIVEYNSTATUS", "FLDFREQUENCY", "FLDWINDOWPERIODBEFORE", "FLDWINDOWPERIODAFTER" };
        string[] alCaptions = { "Code", "Name", "Active Y/N", "Frequency(M)", "Window Period Before(M)", "Window Period After(M)" };


        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixRegistersVesselSurveyFrequency.SurveySearch(
            (int)ViewState["PAGENUMBER"]
            , gvSurveyType.PageSize
            , ref iRowCount
            , ref iTotalPageCount);

        General.SetPrintOptions("gvSurveyType", "Survey Type", alCaptions, alColumns, ds);

        gvSurveyType.DataSource = ds;
        gvSurveyType.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    private void UpdateSurveyType(int SurveyTypeId, string shortcode, string SurveyTypeName, int Activeyn, string Frequency,string WinPeriodBefore , string WinPeriodAfter)
    {

        if (!IsValidText(SurveyTypeName, shortcode, General.GetNullableInteger(Frequency), General.GetNullableInteger(WinPeriodBefore), General.GetNullableInteger(WinPeriodAfter)))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersVesselSurveyFrequency.UpdateSurveyType(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            ,SurveyTypeId
           , General.GetNullableString(shortcode)
           , General.GetNullableString(SurveyTypeName)
           , Activeyn
           , General.GetNullableInteger(Frequency)
           , General.GetNullableInteger(WinPeriodBefore)
           , General.GetNullableInteger(WinPeriodAfter));
    }


    private bool IsValidText(string SurveyType, string ShortCode,int? Frequency,int? Periodbefore,int? Periodafter)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ShortCode.Trim().Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (SurveyType.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";

        if (Frequency.Equals("") || Frequency.Equals(null))
            ucError.ErrorMessage = "Frequency(M) is required.";
        else if (Frequency < 0)
            ucError.ErrorMessage = "Frequency(M) should be greater than or equal to zero";

        if (Periodbefore != 0 && Periodbefore < 0)
            ucError.ErrorMessage = "Window Period Before(M) should be greater than or equal to zero";

        if (Periodafter != 0 && Periodafter < 0)
            ucError.ErrorMessage = "Window Period After(M) should be greater than or equal to zero";

        return (!ucError.IsError);
    }
  
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    protected void gvSurveyType_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSurveyType.CurrentPageIndex + 1;
        BindData();
    }
    protected void Rebind()
    {
        gvSurveyType.SelectedIndexes.Clear();
        gvSurveyType.EditIndexes.Clear();
        gvSurveyType.DataSource = null;
        gvSurveyType.Rebind();
    }
    protected void gvSurveyType_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

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

    protected void gvSurveyType_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {

        if (e.CommandName.ToString().ToUpper() == "DELETE")
        {
            return;
        }
        else if (e.CommandName.ToString().ToUpper() == "ADD")
        {
            try
            {
                string Shortcode = ((RadTextBox)e.Item.FindControl("txtShortCodeAdd")).Text.Trim();
                string SurveyType = ((RadTextBox)e.Item.FindControl("txtSurveyTypeAdd")).Text.Trim();
                string Activeyn = (((RadCheckBox)e.Item.FindControl("cblActiveynAdd")).Checked==true ? "1" : "0").ToString();
                string Frequency = ((UserControlMaskNumber)e.Item.FindControl("txtFrequencyAdd")).Text.Trim();
                string Periodbefore = ((UserControlMaskNumber)e.Item.FindControl("txtWinPeriodBeforeAdd")).Text.Trim();
                string Periodafter = ((UserControlMaskNumber)e.Item.FindControl("txtWinPeriodAfterAdd")).Text.Trim();


                if (!IsValidText(SurveyType, Shortcode, General.GetNullableInteger(Frequency), General.GetNullableInteger(Periodbefore), General.GetNullableInteger(Periodafter)))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersVesselSurveyFrequency.InsertSurveyType(
                      PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Shortcode
                    , SurveyType
                    , int.Parse(Activeyn)
                    , General.GetNullableInteger(Frequency)
                    , General.GetNullableInteger(Periodbefore)
                    , General.GetNullableInteger(Periodafter)
                    );
                
                Rebind();
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvSurveyType_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            UpdateSurveyType(
                    (Convert.ToInt32(((RadLabel)e.Item.FindControl("lblSurveyTypeId")).Text.Trim()))
                     , ((RadTextBox)e.Item.FindControl("txtShortCodeEdit")).Text.Trim()
                     , ((RadTextBox)e.Item.FindControl("txtSurveyTypeEdit")).Text.Trim()
                     , (((RadCheckBox)e.Item.FindControl("cblActiveynEdit")).Checked == true ? 1 : 0)
                     , ((UserControlMaskNumber)e.Item.FindControl("txtFrequencyEdit")).Text.Trim()
                    , ((UserControlMaskNumber)e.Item.FindControl("txtWinPeriodBeforeEdit")).Text.Trim()
                    , ((UserControlMaskNumber)e.Item.FindControl("txtWinPeriodAfterEdit")).Text.Trim());
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSurveyType_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
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

    protected void gvSurveyType_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            ViewState["SurveyTypeId"] = ((RadLabel)e.Item.FindControl("lblSurveyTypeId")).Text;
            RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
            return;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        PhoenixRegistersVesselSurveyFrequency.DeleteSurveyType(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                               , int.Parse(ViewState["SurveyTypeId"].ToString()));
        Rebind();
    }
}
