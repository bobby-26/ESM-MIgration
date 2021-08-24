using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewOperation;
using Telerik.Web.UI;

public partial class Crew_CrewWorkingGearOrderConfirm : System.Web.UI.Page
{
    private string Neededid = string.Empty;
    private string empid = string.Empty;
    private string vslid = string.Empty;
    private string crewplanid = null;
    private string orderid = null;
    private string r = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["crewplanid"] != null && Request.QueryString["crewplanid"] != "")
                ViewState["planid"] = Request.QueryString["crewplanid"];
            if (Request.QueryString["vslid"] != null && Request.QueryString["vslid"] != "")
                ViewState["vslid"] = Request.QueryString["vslid"];
            if (Request.QueryString["empid"] != null && Request.QueryString["empid"] != "")
                ViewState["empid"] = Request.QueryString["empid"];
            if (Request.QueryString["Neededid"] != null && Request.QueryString["Neededid"] != "")
                ViewState["NEEDEDID"] = Request.QueryString["Neededid"];
            if (Request.QueryString["orderid"] != null && Request.QueryString["orderid"] != "")
                ViewState["ORDERID"] = Request.QueryString["orderid"];
            if (Request.QueryString["r"] != null && Request.QueryString["r"] != "")
                ViewState["r"] = Request.QueryString["r"];

            if (ViewState["empid"] != null)
                empid = ViewState["empid"].ToString();
            if (ViewState["vslid"] != null)
                vslid = ViewState["vslid"].ToString();
            if (ViewState["planid"] != null)
                crewplanid = ViewState["planid"].ToString();
            if (ViewState["NEEDEDID"] != null)
                Neededid = ViewState["NEEDEDID"].ToString();
            if (ViewState["ORDERID"] != null)
                orderid = ViewState["ORDERID"].ToString();
            if (ViewState["r"] != null)
                r = ViewState["r"].ToString();

         
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
            
                btnconfirm.Attributes.Add("style", "display:none");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["ORDERSTATUS"] = "";

                DataSet dsCurrency = PhoenixRegistersCurrency.ListCurrency(1, "INR");

                ViewState["Neededid"] = Request.QueryString["Neededid"];
                ViewState["ACTIVE"] = "1";

                EditWorkGearOrder(General.GetNullableGuid(Neededid));

            }

            MainMenu();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private void MainMenu()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();

        toolbarmain.AddButton("Confirm", "CONFIRM", ToolBarDirection.Right);
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuWorkGearGeneral.AccessRights = this.ViewState;
        MenuWorkGearGeneral.MenuList = toolbarmain.Show();
    }

    protected void MenuWorkGearGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {

                if (!IsValidDetail())
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewWorkingGearNeededItem.WGremarkUpdate(General.GetNullableGuid(Neededid)
                                                                 , General.GetNullableString(txtRemarks.Text.Trim())
                                                                 , General.GetNullableInteger(ucCurrency.SelectedCurrency));
               
                ucStatus.Visible = true;
                ucStatus.Text = "Saved Successfully.";
            }
            if (CommandName.ToUpper().Equals("CONFIRM"))
            {
                RadWindowManager1.RadConfirm("Are you sure want to confirm?", "btnconfirm", 320, 150, null, "Confirm");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void btnconfirm_Click(object sender, EventArgs e)
    {
        try
        {
            if (!IsValidDetail())
            {
                ucError.Visible = true;
                return;
            }
            PhoenixCrewWorkingGearNeededItem.WorkingGearConfirm(General.GetNullableGuid(Neededid));
         
            ucStatus.Visible = true;
            ucStatus.Text = "Confirmed Successfully.";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void EditWorkGearOrder(Guid? needid)
    {

        DataTable dt = PhoenixCrewWorkingGearItemIssue.WGNeededItemEdit(needid);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txtTotalAmount.Text = dr["FLDTOTALSTOCKAMOUNT"].ToString();
            txtRemarks.Text = dr["FLDREMARKS"].ToString();
            ucCurrency.SelectedCurrency = dr["FLDCURRENCYID"].ToString();
     
        }
    }
    public bool IsValidDetail()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucCurrency.SelectedCurrency) == null)
            ucError.ErrorMessage = "Currency is required";

        return (!ucError.IsError);

    }


}