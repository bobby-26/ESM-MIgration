using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class PhoenixCrewOffshoreApproval : PhoenixBasePage
{
    private string strEmployeeId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (string.IsNullOrEmpty(Filter.CurrentCrewSelection) && string.IsNullOrEmpty(Filter.CurrentNewApplicantSelection) && string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = string.Empty;
            else if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = Request.QueryString["empid"];
            else if (Request.QueryString["personalmaster"] != null && Request.QueryString["personalmaster"].ToString() != "")
                strEmployeeId = Filter.CurrentCrewSelection;
            else if (Request.QueryString["newapplicant"] != null && Request.QueryString["newapplicant"].ToString() != "")
                strEmployeeId = Filter.CurrentNewApplicantSelection;
            if (!IsPostBack)
            {
                ViewState["VSLID"] = "";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["PAGENUMBERT"] = 1;
                ViewState["SORTEXPRESSIONT"] = null;
                ViewState["SORTDIRECTIONT"] = null;

                ViewState["CVSL"] = -1;
                ViewState["CRNK"] = -1;
                ViewState["Charterer"] = "";
                ViewState["trainingmatrixid"] = "";
                ViewState["offsignerid"] = "";
                ViewState["reliefdate"] = "";
                ViewState["rankid"] = "";
                ViewState["vesselid"] = "";
                ViewState["vsltype"] = "";
                ViewState["empid"] = strEmployeeId;

                if (Request.QueryString["vslid"] != null)
                    ViewState["VSLID"] = Request.QueryString["vslid"];

                if (Request.QueryString["empid"] != null)
                    ViewState["empid"] = Request.QueryString["empid"];

                if (Request.QueryString["rankid"] != null && Request.QueryString["rankid"].ToString() != "")
                {
                    ViewState["rankid"] = Request.QueryString["rankid"];
                }
                if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"].ToString() != "")
                {
                    ViewState["vesselid"] = Request.QueryString["vesselid"].ToString();
                }

                if (Request.QueryString["reliefdate"] != null && Request.QueryString["reliefdate"].ToString() != "")
                {
                    ViewState["reliefdate"] = Request.QueryString["reliefdate"].ToString();
                }

                if (Request.QueryString["offsignerid"] != null && Request.QueryString["offsignerid"].ToString() != "")
                    ViewState["offsignerid"] = Request.QueryString["offsignerid"].ToString();

                if (Request.QueryString["trainingmatrixid"] != null && Request.QueryString["trainingmatrixid"].ToString() != "")
                    ViewState["trainingmatrixid"] = Request.QueryString["trainingmatrixid"].ToString();

                if (Request.QueryString["crewplanid"] != null && Request.QueryString["crewplanid"].ToString() != "")
                    ViewState["crewplanid"] = Request.QueryString["crewplanid"].ToString();

                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

                BindTrainingMatrix();
                BindOffSigner();

                BindFields();
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Confirm", "CONFIRMAPPROVE", ToolBarDirection.Right);
            toolbar.AddButton("Back", "BACK",ToolBarDirection.Right);
            
            CrewMenuGeneral.AccessRights = this.ViewState;
            CrewMenuGeneral.MenuList = toolbar.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindFields()
    {
        BindRank();
        if (ViewState["rankid"] != null && ViewState["rankid"].ToString() != "")
            ddlRank.SelectedValue = ViewState["rankid"].ToString();

        if (ViewState["vesselid"] != null && ViewState["vesselid"].ToString() != "")
        {
            ucVessel.SelectedVessel = ViewState["vesselid"].ToString();
            ucVessel.bind();
            SetVesselType(null, null);
        }

        if (ViewState["reliefdate"] != null && ViewState["reliefdate"].ToString() != "")
            ucDate.Text = ViewState["reliefdate"].ToString();

        if (ViewState["offsignerid"] != null && ViewState["offsignerid"].ToString() != "")
            ddlOffSigner.SelectedValue = ViewState["offsignerid"].ToString();

        if (ViewState["trainingmatrixid"] != null && ViewState["trainingmatrixid"].ToString() != "")
            ddlTrainingMatrix.SelectedValue = ViewState["trainingmatrixid"].ToString();
    }

    protected void ddlRank_Changed(object sender, EventArgs e)
    {
        BindTrainingMatrix();
        BindOffSigner();
    }

    protected void SetVesselType(object sender, EventArgs e)
    {
        ViewState["Charterer"] = "";
        if (General.GetNullableInteger(ucVessel.SelectedVessel) != null || General.GetNullableInteger(Request.QueryString["vesselid"]) != null)
        {
            string vesselid = General.GetNullableInteger(ucVessel.SelectedVessel) == null ? Request.QueryString["vesselid"] : ucVessel.SelectedVessel;
            DataSet ds = PhoenixRegistersVessel.EditVessel(int.Parse(vesselid));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ucVesselType.SelectedVesseltype = ds.Tables[0].Rows[0]["FLDTYPE"].ToString();
                ViewState["vsltype"] = ds.Tables[0].Rows[0]["FLDTYPE"].ToString();
                ViewState["Charterer"] = ds.Tables[0].Rows[0]["FLDCHARTERER"].ToString();
            }
            BindTrainingMatrix();
            BindOffSigner();
        }
    }

    public void BindRank()
    {
        try
        {
            DataTable dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(ViewState["empid"].ToString()));
            if (dt.Rows.Count > 0)
            {
                ddlRank.DataSource = PhoenixCrewManagement.CrewRankDeptList(General.GetNullableInteger(dt.Rows[0]["FLDGROUP"].ToString()), null);
                ddlRank.DataBind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindTrainingMatrix()
    {
        ddlTrainingMatrix.Items.Clear();
        ddlTrainingMatrix.DataSource = PhoenixCrewOffshoreTrainingMatrix.CrewOffshoreTrainingMatrixList(
                                            General.GetNullableInteger(ViewState["vsltype"].ToString()),
                                            General.GetNullableInteger(ddlRank.SelectedValue),
                                            General.GetNullableInteger(ViewState["Charterer"].ToString()));
        ddlTrainingMatrix.DataTextField = "FLDMATRIXNAME";
        ddlTrainingMatrix.DataValueField = "FLDMATRIXID";
        ddlTrainingMatrix.DataBind();
        ddlTrainingMatrix.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

        if (Request.QueryString["trainingmatrixid"] != null && Request.QueryString["trainingmatrixid"].ToString() != "")
        {
            if (ddlTrainingMatrix.Items.FindItemByValue(Request.QueryString["trainingmatrixid"].ToString()) != null)
                ddlTrainingMatrix.SelectedValue = Request.QueryString["trainingmatrixid"].ToString();
        }
        else
        {
            if (ddlTrainingMatrix.Items.Count == 2)
                ddlTrainingMatrix.SelectedIndex = 1;

        }
    }

    protected void BindOffSigner()
    {
        UserControlVessel vsl = ucVessel;
        RadComboBox rank = ddlRank;
        RadComboBox cob = ddlOffSigner;

        int? VesselId = General.GetNullableInteger(vsl.SelectedVessel);
        int? RankId = General.GetNullableInteger(rank.SelectedValue);

        cob.Items.Clear();

        if (VesselId.HasValue)
        {
            bool bind = false;
            if (int.Parse(ViewState["CVSL"].ToString()) != VesselId.Value)
            {
                ViewState["CVSL"] = VesselId.Value;
                bind = true;
            }
            if (RankId.HasValue && int.Parse(ViewState["CRNK"].ToString()) != RankId.Value)
            {
                ViewState["CRNK"] = RankId.Value;
                bind = true;
            }
            if (bind)
            {
                cob.DataSource = PhoenixCrewOffshoreReliefRequest.ListRelivee(VesselId.Value, RankId,General.GetNullableInteger(strEmployeeId));
            }
        }
        else
            cob.DataSource = PhoenixCrewOffshoreReliefRequest.ListRelivee(General.GetNullableInteger(vsl.SelectedVessel), 
                General.GetNullableInteger(rank.SelectedValue),
                General.GetNullableInteger(strEmployeeId));
        cob.DataTextField = "FLDEMPLOYEENAME";
        cob.DataValueField = "FLDEMPLOYEEID";
        cob.DataBind();
        cob.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

        if (Request.QueryString["offsignerid"] != null && Request.QueryString["offsignerid"].ToString() != "")
        {
            if (cob.Items.FindItemByValue(Request.QueryString["offsignerid"].ToString()) != null)
                cob.SelectedValue = Request.QueryString["offsignerid"].ToString();
        }
    }

    protected void CrewMenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;


            if (CommandName.ToUpper().Equals("BACK"))
            {
                ViewState["rankid"] = General.GetNullableInteger(ddlRank.SelectedValue);
                ViewState["vesselid"] = General.GetNullableInteger(ucVessel.SelectedVessel);
                ViewState["reliefdate"] = General.GetNullableDateTime(ucDate.Text);
                ViewState["offsignerid"] = General.GetNullableInteger(ddlOffSigner.SelectedValue);
                ViewState["trainingmatrixid"] = General.GetNullableInteger(ddlTrainingMatrix.SelectedValue);

                 Response.Redirect("../CrewOffshore/CrewOffshoreSuitabilityCheck.aspx?empid=" + ViewState["empid"]
                //Response.Redirect("../CrewOffshore/CrewOffshoreSuitabilityCheckWithDocument.aspx?empid=" + ViewState["empid"]
                     + "&crewplanid=" + ViewState["crewplanid"].ToString()
                    + "&approval=1"
                    + "&rankid=" + ViewState["rankid"]
                    + "&vesselid=" + ViewState["vesselid"]
                    + "&reliefdate=" + ViewState["reliefdate"]
                    + "&offsignerid=" + ViewState["offsignerid"]
                    + "&trainingmatrixid=" + ViewState["trainingmatrixid"]
                    + "&personalmaster=" + Request.QueryString["personalmaster"]
                    + "&newapplicant=" + Request.QueryString["newapplicant"]
                    + "&popup=" + Request.QueryString["popup"], false);

                //Response.Redirect("../CrewOffshore/CrewOffshoreSuitabilityCheck.aspx" 
                //    + (Request.QueryString.ToString() != string.Empty ? ("?" + Request.QueryString.ToString()) : string.Empty), false);
            }
            else if (CommandName.ToUpper().Equals("CONFIRMAPPROVE"))
            {
                if (!IsValidProposal())
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewOffshoreApproveProposal.ApproveProposal(General.GetNullableGuid(ViewState["crewplanid"].ToString()), txtRemaks.Text,
                    General.GetNullableInteger(ucSignOnReason.SelectedSignOnReason));

                PhoenixCrewOffshoreCheckList.InsertDocumentCheckList(General.GetNullableGuid(ViewState["crewplanid"].ToString()), 1);

                ucStatus.Text = "This candidate is approved for the vessel successfully.";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                  "BookMarkScript", "fnReloadList('codehelpsuitability', null, null);", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidProposal()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ViewState["vesselid"].ToString()) == null)
            ucError.ErrorMessage = "Vessel is required.";

        if (General.GetNullableInteger(ViewState["rankid"].ToString()) == null)
            ucError.ErrorMessage = "Rank is required.";

        if (General.GetNullableDateTime(ViewState["reliefdate"].ToString()) == null)
            ucError.ErrorMessage = "Expected Joining Date is required.";

        if (General.GetNullableInteger(ViewState["trainingmatrixid"].ToString()) == null)
            ucError.ErrorMessage = "Training Matrix is required.";

        if (General.GetNullableString(txtRemaks.Text) == null)
            ucError.ErrorMessage = "Approval Remarks is required.";

        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
    }   
}
