using System;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.Registers;

public partial class CrewOffshore_CrewOffshoreBudgetAddEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        //toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
        MenuCrewBudget.AccessRights = this.ViewState;
        MenuCrewBudget.MenuList = toolbar.Show();

       
        if (!IsPostBack)
        {
            ViewState["id"] = "";
            ViewState["vessel"] = "";
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                ViewState["id"] = Request.QueryString["id"].ToString();
                EditBudget();
            }
            if (Request.QueryString["vessel"] != null && Request.QueryString["vessel"] != "")
            {
                ViewState["vessel"] = Request.QueryString["vessel"].ToString();
            }
        }


    }
    public void EditBudget()
    {
        txtRemarks.EditModes = EditModes.All;
        txtOfferRemarks.EditModes = EditModes.All;
        DataTable dt = PhoenixRegistersVesselBudget.EditCrewRevitionBudget(new Guid(ViewState["id"].ToString()));
        //ViewState["VESSELID"].ToString()
        ucEffectiveDate.Text = dt.Rows[0]["FLDEFFECTIVEDATE"].ToString();
        ucCurrency.SelectedCurrency = dt.Rows[0]["FLDCURRENCY"].ToString();
        ucOverlapWage.Text = dt.Rows[0]["FLDOVERLAPWAGE"].ToString();
        ucOtherAllowance.Text = dt.Rows[0]["FLDOVERLAPWAGE"].ToString();
        ucTankCleanAllowance.Text = dt.Rows[0]["FLDTANKCLEANALLOWANCE"].ToString();
        ucDPAllowance.Text = dt.Rows[0]["FLDDPALLOWANCE"].ToString();
        txtRemarks.Content = dt.Rows[0]["FLDREMARKS"].ToString();
        txtOfferRemarks.Content = dt.Rows[0]["FLDOFFERLETTERREMARKS"].ToString();
        

    }
    protected void MenuCrewBudget_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string scriptClosePopup = "";
                scriptClosePopup += "<script language='javaScript' id='Budget'>" + "\n";
                scriptClosePopup += "fnReloadList('City');";
                scriptClosePopup += "</script>" + "\n";
                if (!IsValidBudgedvessel(ViewState["vessel"].ToString()))
                {
                    ucError.Visible = true;
                    return;
                }
                if (!IsValidData(ucEffectiveDate.Text
                                  , ucOtherAllowance.Text
                                  , txtRemarks.Text
                      ))
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["id"].ToString() == string.Empty)
                {
                    PhoenixRegistersVesselBudget.InsertBudgetRevision(int.Parse(ViewState["vessel"].ToString()),
                   DateTime.Parse(ucEffectiveDate.Text)
                   , General.GetNullableInteger(ucCurrency.SelectedCurrency)
                   , General.GetNullableDecimal(ucOverlapWage.Text)
                   , General.GetNullableDecimal(ucOtherAllowance.Text)
                   , General.GetNullableDecimal(ucTankCleanAllowance.Text)
                   , General.GetNullableDecimal(ucDPAllowance.Text)
                   , General.GetNullableString(txtRemarks.Content)
                   , General.GetNullableString(txtOfferRemarks.Content)
                   );
                }
                else
                {
                    Guid revisionid = new Guid(ViewState["id"].ToString());

                    PhoenixRegistersVesselBudget.UpdateBudgetRevision(revisionid
                        , int.Parse(ViewState["vessel"].ToString())
                        , DateTime.Parse(ucEffectiveDate.Text)
                        , General.GetNullableInteger(ucCurrency.SelectedCurrency)
                            , General.GetNullableDecimal(ucOverlapWage.Text)
                            , General.GetNullableDecimal(ucOtherAllowance.Text)
                            , General.GetNullableDecimal(ucTankCleanAllowance.Text)
                            , General.GetNullableDecimal(ucDPAllowance.Text)
                            , General.GetNullableString(txtRemarks.Content)
                            , General.GetNullableString(txtOfferRemarks.Content));                  

                }
                Page.ClientScript.RegisterStartupScript(typeof(Page), "Budget", scriptClosePopup);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidBudgedvessel(string vesselid)
    {
        if (General.GetNullableString(vesselid) == null)
            ucError.ErrorMessage = "Please switch the vessel";

        return (!ucError.IsError);
    }
    private bool IsValidData(string effectivedate, string otherallowances, string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(effectivedate) == null)
            ucError.ErrorMessage = "Effective Date is required.";

        if (General.GetNullableDecimal(otherallowances) != null)
        {
            if (General.GetNullableString(remarks) == null)
                ucError.ErrorMessage = "Remarks is required.";
        }

        return (!ucError.IsError);
    }

}