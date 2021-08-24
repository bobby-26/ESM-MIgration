using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using SouthNests.Phoenix.CrewOperation;
using Telerik.Web.UI;
public partial class CrewStatus : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            CrewMenuGeneral.AccessRights = this.ViewState;
            CrewMenuGeneral.MenuList = toolbarsub.Show();

            SetEmployeePrimaryDetails();
            imgZone.Attributes.Add("onclick", "parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewZoneHistory.aspx');return false;");
        }

        BindDataPlannedReliever();
        BindDataNextPlan();
        BindCrewInActive();
        BindCrewRem();
        BindCrewNTBR();
        EditCrewStatus();
    }

    private void EditCrewStatus()
    {
        DataSet ds = PhoenixCrewStatus.CrewStatusEdit(
                Convert.ToInt32(Filter.CurrentCrewSelection));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];

            txtDateOfTeleConf.Text = String.Format("{0:dd/MM/yyyy}", dt.Rows[0]["FLDTELEONFERENCEDATE"]);
            txtDOA.Text = String.Format("{0:dd/MM/yyyy}", dt.Rows[0]["FLDDOA"]);
            txtDOAGiven.Text = String.Format("{0:dd/MM/yyyy}", dt.Rows[0]["FLDDOAGIVENDATE"]);
            txtDOAMethod.Text = dt.Rows[0]["FLDDOAMETHODNAME"].ToString();
            txtExpectedJoin.Text = String.Format("{0:dd/MM/yyyy}", dt.Rows[0]["FLDEXPECTEDJOININGDATE"]);
            txtFirstSignedOn.Text = String.Format("{0:dd/MM/yyyy}", dt.Rows[0]["FLDFIRSTSIGNON"]);
            txtFromPresentRank.Text = String.Format("{0:dd/MM/yyyy}", dt.Rows[0]["FLDPROMOTIONDATE"]);
            txtInActiveRemaks.Text = dt.Rows[0]["FLDNTBRREMARKS"].ToString();
            txtPromDate.Text = String.Format("{0:dd/MM/yyyy}", dt.Rows[0]["FLDPROMOTIONDATE"]);
            txtReliefDue.Text = String.Format("{0:dd/MM/yyyy}", dt.Rows[0]["FLDRELEFDUEDATE"]);
            txtSignedOffReason.Text = dt.Rows[0]["FLDSIGNOFFREASON"].ToString();
            txtSignOffDate.Text = String.Format("{0:dd/MM/yyyy}", dt.Rows[0]["FLDSIGNOFFDATE"]);
            txtSignOffVessel.Text = dt.Rows[0]["FLDSIGNOFFVESSELNAME"].ToString();
            txtSignOnDate.Text = String.Format("{0:dd/MM/yyyy}", dt.Rows[0]["FLDSIGNONDATE"]);
            txtSignOnVessel.Text = dt.Rows[0]["FLDSIGNONVESSELNAME"].ToString();
            txtStandBy.Text = String.Format("{0:dd/MM/yyyy}", dt.Rows[0]["FLDSTANDBYDATE"]);
            txtStatus.Text = dt.Rows[0]["FLDEMPLOYEESTATUSNAME"].ToString();
        }
    }

    protected void CrewMenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtZone.Text = dt.Rows[0]["FLDZONENAME"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindDataPlannedReliever()
    {
        try
        {
            DataSet ds = PhoenixCrewStatus.CrewStatusEdit(
                Convert.ToInt32(Filter.CurrentCrewSelection));

            gvPlannedReliever.DataSource = ds.Tables[1];
            gvPlannedReliever.DataBind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPlannedReliever_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDataPlannedReliever();
    }

    protected void gvPlannedReliever_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }


    private void BindCrewRem()
    {
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string menucode = Filter.CurrentMenuCodeSelection;

        string vesselid = string.Empty;
        NameValueCollection nvc = Filter.CrewReimbursementFilterSelection;

        DataTable dt = PhoenixCrewStatus.CrewReimbursementStatus(Convert.ToInt32(Filter.CurrentCrewSelection));
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        gvRem.DataSource = dt;
        gvRem.DataBind();

    }


    protected string GetName(string val)
    {
        string result = string.Empty;
        if (val == "1")
            result = "Reimbursement(B.O.C)";
        else if (val == "2")
            result = "Reimbursement(Monthly / OneTime)";
        else if (val == "3")
            result = "Reimbursement(E.O.C)";
        else if (val == "-1")
            result = "Recovery(B.O.C)";
        else if (val == "-2")
            result = "Recovery(Monthly / OneTime)";
        else if (val == "-3")
            result = "Recovery(E.O.C)";
        return result;
    }

    protected void gvRem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindCrewRem();
    }

    protected void gvRem_ItemDataBound(object sender, GridItemEventArgs e)
    {
       

        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;

            DataRowView drv = (DataRowView)item.DataItem;

            LinkButton refno = (LinkButton)item.FindControl("lblRefNo");
            if (refno != null)
                refno.Attributes.Add("onclick", "parent.openNewWindow('Details', '', '" + Session["sitepath"] + "/Crew/CrewReimbursementDetail.aspx?rembid=" + drv["FLDREIMBURSEMENTID"] + "&readonly=true');return false;");



            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblApprovedAmount");
            UserControlToolTip uct = (UserControlToolTip)item.FindControl("ucToolTip");
            lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
        }

    }

    protected void gvInterviewBy_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    
    private void BindCrewNTBR()
    {
        DataSet dsNTBRManager = PhoenixCrewNTBR.CrewNTBRList(General.GetNullableInteger(Filter.CurrentCrewSelection));
        gvNTBRManager.DataSource = dsNTBRManager;
        gvNTBRManager.DataBind();
        
    }

    protected void gvNTBRManager_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindCrewNTBR();
    }

    private void BindCrewInActive()
    {
        DataSet ds = PhoenixCrewActive.CrewActiveInactiveHistory(General.GetNullableInteger(Filter.CurrentCrewSelection));

        gvCrewInActive.DataSource = ds;
        gvCrewInActive.DataBind();
    }

    protected void gvCrewInActive_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindCrewInActive();
    }

    private void BindDataNextPlan()
    {
        try
        {
            DataSet ds = PhoenixCrewStatus.CrewStatusEdit(
                Convert.ToInt32(Filter.CurrentCrewSelection));

            gvNextPlan.DataSource = ds.Tables[2];
            gvNextPlan.DataBind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvNextPlan_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

    protected void gvNextPlan_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDataNextPlan();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        SetEmployeePrimaryDetails();
    }




}
