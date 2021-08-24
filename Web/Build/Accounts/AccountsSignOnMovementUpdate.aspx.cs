using System;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Collections;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Integration;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.CrewManagement;
public partial class Accounts_AccountsSignOnMovementUpdate : System.Web.UI.Page
{
    string header = "Please provide the following required information", error = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);

        MenuMovementUpdate.AccessRights = this.ViewState;
        MenuMovementUpdate.MenuList = toolbarmain.Show();
        if (!IsPostBack)
        {
            ViewState["SIGNONOFFID"] = "";
            if (Request.QueryString["SIGNONID"] != null && Request.QueryString["SIGNONID"].ToString() != string.Empty)
            {
                ViewState["SIGNONOFFID"] = Request.QueryString["SIGNONID"].ToString();

            }

            ViewState["VESSELID"] = "";
            if (Request.QueryString["VESSELID"] != null && Request.QueryString["VESSELID"].ToString() != string.Empty)
            {
                ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();

            }

            ViewState["EMPLOYEEID"] = "";
            if (Request.QueryString["EMPLOYEEID"] != null && Request.QueryString["EMPLOYEEID"].ToString() != string.Empty)
            {
                ViewState["EMPLOYEEID"] = Request.QueryString["EMPLOYEEID"].ToString();
                CrewContract.employeeId = ViewState["EMPLOYEEID"].ToString();
            }


            if (ViewState["SIGNONOFFID"] != null)
                BindDetails();
        }
    }
    protected void RegistersMovementUpdate_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            string scriptClosePopup = "";
            scriptClosePopup += "<script language='javaScript' id='City'>" + "\n";
            scriptClosePopup += "fnReloadList('City');";
            scriptClosePopup += "</script>" + "\n";

            string scriptRefreshDontClose = "";
            scriptRefreshDontClose += "<script language='javaScript' id='CityNew'>" + "\n";
            scriptRefreshDontClose += "fnReloadList('OfficeStaff', null, 'yes');";
            scriptRefreshDontClose += "</script>" + "\n";

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                //if (!IsValidMovementLog())
                //{
                //    ucError.Visible = true;
                //    return;
                //}
                if (IsValidMovementLog(ref header, ref error))
                {
                    RadWindowManager1.RadAlert(error, 350, 175, header, null);
                    //ucError.Visible = true;
                    return;
                }

                PhoenixAccountsSignonoffConfirm.SignOffMovementLogInsert(
                                                 int.Parse(ddlMovement.SelectedMovement)
                                                , int.Parse(ViewState["SIGNONOFFID"].ToString())
                                                , int.Parse(ViewState["EMPLOYEEID"].ToString())
                                                , int.Parse(ucRank.SelectedRank)
                                                , General.GetNullableGuid(CrewContract.SelectedContract)
                                                , DateTime.Parse(ucFromdate.Text)
                                                , DateTime.Parse(ucTodate.Text)
                                                , int.Parse(ViewState["VESSELID"].ToString()));

                ucStatus.Text = "Information Added";
                String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    //public bool IsValidMovementLog()
    //{
    //    ucError.HeaderMessage = "Please provide the following required information";

    //    if (int.Parse(ddlMovement.SelectedMovement).ToString() == null)
    //        ucError.ErrorMessage = "Movement is required";

    //    if (int.Parse(ucRank.SelectedRank).ToString() == null)
    //        ucError.ErrorMessage = "Rank is required";

    //    if (General.GetNullableString(CrewContract.SelectedContract) == null)
    //        ucError.ErrorMessage = "Contract is required";

    //    if (int.Parse(ddlVessel.SelectedVessel).ToString() == null)
    //        ucError.ErrorMessage = "Vessel is required";

    //    if (General.GetNullableDateTime(ucFromdate.Text) == null)
    //        ucError.ErrorMessage = "From date is required.";

    //    if (General.GetNullableDateTime(ucTodate.Text) == null)
    //        ucError.ErrorMessage = "To date is required.";

    //    return (!ucError.IsError);

    //}

    private bool IsValidMovementLog(ref string headermessage, ref string errormessage)
    {
        //ucError.HeaderMessage = "Please provide the following required information";
        errormessage = "";
        //ucError.HeaderMessage = "Please provide the following required information";

        if ((int.Parse(ddlMovement.SelectedMovement).ToString() == null))
            errormessage = errormessage + "Movement is required.</br>";

        if ((int.Parse(ucRank.SelectedRank).ToString() == null))
            errormessage = errormessage + "Rank is required.</br>";

        if (((CrewContract.SelectedContract) == ""))
            errormessage = errormessage + "Contract is required.</br>";

        if ((int.Parse(ddlVessel.SelectedVessel).ToString() == null))
            errormessage = errormessage + "Vessel is required.</br>";

        if (((ucFromdate.Text) == null))
            errormessage = errormessage + "From date is required.</br>";

        if (((ucTodate.Text) == null))
            errormessage = errormessage + "To date is required.</br>";


        if (!errormessage.Length.Equals(0))
        {
            return true;
        }
        else
            return false;

    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            ViewState["SIGNONID"] = null;
            ViewState["EMPLOYEEID"] = null;
            ViewState["VESSELID"] = null;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindDetails()
    {
        DataSet ds = PhoenixAccountsSignonoffConfirm.AccountssignonmovementEdit(int.Parse(ViewState["SIGNONOFFID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {

            DataRow dr = ds.Tables[0].Rows[0];
            txtName.Text = dr["FLDEMPLOYEENAME"].ToString();
            ucRank.SelectedRank = dr["FLDSIGNONRANKID"].ToString();
            ddlVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
            txtsignondate.Text = string.Format("{0:dd/MMM/yyyy}", dr["FLDSIGNONDATE"].ToString());
            ddlMovement.SelectedMovement = dr["FLDMOVEMENTID"].ToString();
            CrewContract.SelectedContract = dr["FLDCONTRACTID"].ToString();
            CrewContract.Text = dr["FLDCONTRACTDETAIL"].ToString();
            ucFromdate.Text = dr["FLDFROMDATE"].ToString();
            ucTodate.Text = dr["FLDTODATE"].ToString();

        }
    }
    private void Reset()
    {

        ViewState["SIGNONID"] = null;
        ViewState["EMPLOYEEID"] = null;
        ViewState["VESSELID"] = null;
        ddlMovement.SelectedMovement = "";
        ucRank.SelectedRank = "";
        CrewContract.SelectedContract = "";
        ddlVessel.SelectedVessel = "";

    }

}