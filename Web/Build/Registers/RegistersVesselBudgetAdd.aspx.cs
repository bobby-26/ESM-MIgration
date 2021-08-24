using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;

public partial class RegistersVesselBudgetAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuDetails.AccessRights = this.ViewState;
            MenuDetails.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                cblNationality.DataSource = PhoenixRegistersCountry.ListNationality();
                cblNationality.DataTextField = "FLDNATIONALITY";
                cblNationality.DataValueField = "FLDCOUNTRYCODE";
                cblNationality.DataBind();

                ViewState["REVISIONID"] = "";
                ViewState["BUDGETID"] = "";

                if (Request.QueryString["revisionid"] != null && Request.QueryString["revisionid"].ToString() != "")
                    ViewState["REVISIONID"] = Request.QueryString["revisionid"].ToString();
                if (Request.QueryString["Budgetid"] != null && Request.QueryString["Budgetid"].ToString() != "")
                {
                    ViewState["BUDGETID"] = Request.QueryString["Budgetid"].ToString();
                    DataTable dt = PhoenixRegistersVesselBudget.EditBudget(new Guid(ViewState["BUDGETID"].ToString()));
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows.Count == 1)
                        {
                            ucRankAdd.SelectedRank = dt.Rows[0]["FLDRANKID"].ToString();
                            ucWage.Text = dt.Rows[0]["FLDBUDGETEDWAGE"].ToString();
                            string[] slist = dt.Rows[0]["FLDPREFERREDNATIONALITY"].ToString().Split(',');
                            foreach (RadListBoxItem li in cblNationality.Items)
                            {
                                foreach (string s in slist)
                                {
                                    if (li.Value.Equals(s))
                                    {
                                        li.Checked = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuDetails_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidBudget(ucRankAdd.SelectedRank, ucWage.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                StringBuilder strList = new StringBuilder();

                foreach (RadListBoxItem li in cblNationality.Items)
                {

                    if (li.Checked == true)
                    {
                        strList.Append("," + li.Value + ",");
                    }
                }
                string strNationality = strList.ToString().Replace(",,", ",");
                if (ViewState["BUDGETID"] == null || ViewState["BUDGETID"].ToString() == "")
                {
                    PhoenixRegistersVesselBudget.InsertBudget(int.Parse(Filter.CurrentVesselMasterFilter), new Guid(ViewState["REVISIONID"].ToString())
                                                                , int.Parse(ucRankAdd.SelectedRank), decimal.Parse(ucWage.Text), null
                                                                , General.GetNullableString(strNationality), null, null);
                }
                else
                {

                    PhoenixRegistersVesselBudget.UpdateBudget(new Guid(ViewState["BUDGETID"].ToString()), int.Parse(Filter.CurrentVesselMasterFilter)
                                                                , int.Parse(ucRankAdd.SelectedRank), decimal.Parse(ucWage.Text), null
                                                                , General.GetNullableString(strNationality), null, null);
                }

                ucStatus.Text = "Budget saved successfully.";
                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>";
                Script += "fnReloadList();";
                Script += "</script>";
                RadScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", Script, true);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    private bool IsValidBudget(string rankid, string wage)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(ViewState["REVISIONID"].ToString()) == null)
            ucError.ErrorMessage = "Please select the revision to add budget.";

        if (General.GetNullableInteger(rankid) == null)
            ucError.ErrorMessage = "Rank is required.";

        if (General.GetNullableDecimal(wage) == null)
            ucError.ErrorMessage = "Budgeted Wage/day is required.";

        return (!ucError.IsError);
    }


}
