using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using Telerik.Web.UI;
public partial class InspectionMOCExtentionActionPlanEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);
            MenuCARGeneral.AccessRights = this.ViewState;
            MenuCARGeneral.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["MOCID"] = "";
                ViewState["ACTIONPLANID"] = "";
                ViewState["MOCEXTENTIONID"] = "";

                if (Request.QueryString["MOCID"] != null && Request.QueryString["MOCID"].ToString() != string.Empty)
                    ViewState["MOCID"] = Request.QueryString["MOCID"].ToString();

                if (Request.QueryString["VESSELID"] != null && Request.QueryString["VESSELID"].ToString() != string.Empty)
                    ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();

                if (Request.QueryString["MOCEXTENTIONID"] != null && Request.QueryString["MOCEXTENTIONID"].ToString() != string.Empty)
                    ViewState["MOCEXTENTIONID"] = Request.QueryString["MOCEXTENTIONID"].ToString();

                if (Request.QueryString["ACTIONPLANID"] != null && Request.QueryString["ACTIONPLANID"].ToString() != string.Empty)
                {
                    ViewState["ACTIONPLANID"] = Request.QueryString["ACTIONPLANID"].ToString();
                    BindAction();
                }

            }
            BindCrewList();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindAction()
    {
        if (ViewState["ACTIONPLANID"] != null && ViewState["ACTIONPLANID"].ToString() != string.Empty)
        {
            DataSet ds = PhoenixInspectionMOCActionPlan.MOCActionPlanEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                               , General.GetNullableGuid((ViewState["ACTIONPLANID"]).ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                string deparmenttypeid = "";
                string departmenttype = "";
                DataRow dr = ds.Tables[0].Rows[0];

                txtActionToBeTaken.Text = dr["FLDACTIONTOBETAKEN"].ToString();
                txtTargetdateEdit.Text = dr["FLDTARGETDATE"].ToString();
                ucDepartmentedit.DataBind();
                ucDepartmentedit.SelectedDepartment = dr["FLDDEPARTMENTID"].ToString();

                deparmenttypeid = dr["FLDDEPARTMENTID"].ToString();
                lbldept.Text = dr["FLDDEPARTMENTID"].ToString();
                lbldepartmentid.Text = dr["FLDDEPARTMENTTYPEID"].ToString();
                lblVesselId.Text = ViewState["VESSELID"].ToString();

                if (lbldept.Text != "")
                {
                    DataSet ds1 = PhoenixRegistersDepartment.EditDepartment(int.Parse(lbldept.Text));

                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr1 = ds.Tables[0].Rows[0];
                        departmenttype = dr1["FLDDEPARTMENTTYPEID"].ToString();
                        ViewState["DEPARTMENTTYPE"] = departmenttype;
                    }
                }
                if (departmenttype == "2")
                {
                    txtPersonNameEdit.Text = dr["FLDPICNAME"].ToString();
                    txtPersonOfficeIdEdit.Text = dr["FLDPERSONINCHARGE"].ToString();
                    txtPersonRankEdit.Text = dr["FLDPICRANK"].ToString();
                }
                else if (departmenttype == "1")
                {
                    txtCrewIdEdit.Text = dr["FLDPERSONINCHARGE"].ToString();
                    txtCrewNameEdit.Text = dr["FLDPICNAME"].ToString();
                    txtCrewRankEdit.Text = dr["FLDPICRANK"].ToString();
                }
                else
                {
                    actionplancrewedit.Visible = false;
                    actionplanofficeedit.Visible = false;
                }
            }
        }
    }
    private void BindCrewList()
    {
        string deparmenttypeid = "";
        DataSet ds;

        if (ucDepartmentedit.SelectedDepartment != "")
        {
            ds = PhoenixRegistersDepartment.EditDepartment(int.Parse(ucDepartmentedit.SelectedDepartment));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                deparmenttypeid = dr["FLDDEPARTMENTTYPEID"].ToString();
                ViewState["DEPARTMENTTYPE"] = deparmenttypeid;
            }
        }

        if ((deparmenttypeid == "") || (deparmenttypeid == "Dummy"))
        {
            actionplancrewedit.Visible = false;
            actionplanofficeedit.Visible = false;
        }

        if (deparmenttypeid == "2")
        {
            actionplancrewedit.Visible = false;
            actionplanofficeedit.Visible = true;
            ViewState["DEPARTMENTID"] = ucDepartmentedit.SelectedDepartment;

            if (imgPersonOfficeEdit != null)
            {
                imgPersonOfficeEdit.Attributes.Add("onclick", "return showPickList('spnActionPlanPersonOfficeEdit', 'codehelp1', '', '../Common/CommonPickListUser.aspx?Departmentid=" + ViewState["DEPARTMENTID"].ToString() + "&MOC=true', true);");
            }
        }

        if (deparmenttypeid == "1")
        {
            actionplancrewedit.Visible = true;
            actionplanofficeedit.Visible = false;
            if (imgPersonInChargeEdit != null)
            {
                imgPersonInChargeEdit.Attributes.Add("onclick", "return showPickList('spnPersonInChargeactionplanEdit', 'codehelp1', '','../Common/CommonPickListVesselCrewListByDate.aspx?VesselId="
                                                + ViewState["VESSELID"].ToString().ToString() + "', true); ");
            }
        }
    }
    protected void MenuCARGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Inspection/InspectionMOCExtentionAdd.aspx?&VESSELID=" + ViewState["VESSELID"].ToString() + "&MOCID=" + ViewState["MOCID"].ToString() + "&MOCEXTENTIONID=" + ViewState["MOCEXTENTIONID"].ToString());
            }
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["ACTIONPLANID"] == null || ViewState["ACTIONPLANID"].ToString().Equals(""))
                    InsertCorrectiveAction();
                else
                    UpdateCorrectiveAction();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void InsertCorrectiveAction()
    {

        if (Request.QueryString["EXTN"] != null && Request.QueryString["EXTN"].ToString() == "1" && ViewState["MOCEXTENTIONID"].ToString().Equals(""))
        {
            ucError.ErrorMessage = "Please save extention details first";
            ucError.Visible = true;
            return;
        }

        Guid? actionplanid = General.GetNullableGuid(null);
        PhoenixInspectionMOCActionPlan.MOCActionPlanInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                      , ref actionplanid
                                                                      , new Guid((ViewState["MOCID"]).ToString())
                                                                      , General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                                                      , General.GetNullableGuid(null)
                                                                      , General.GetNullableGuid(ViewState["MOCEXTENTIONID"].ToString())
                                                                      , txtActionToBeTaken.Text
                                                                      , (ViewState["DEPARTMENTTYPE"].ToString() == "2") ? General.GetNullableInteger(txtPersonOfficeIdEdit.Text) : General.GetNullableInteger(txtCrewIdEdit.Text)
                                                                      , (ViewState["DEPARTMENTTYPE"].ToString() == "2") ? (txtPersonNameEdit.Text) : txtCrewNameEdit.Text
                                                                      , (ViewState["DEPARTMENTTYPE"].ToString() == "2") ? (txtPersonRankEdit.Text) : txtCrewRankEdit.Text
                                                                      , null
                                                                      , DateTime.Parse(txtTargetdateEdit.Text)
                                                                      , General.GetNullableDateTime(null)
                                                                      , General.GetNullableInteger(ucDepartmentedit.SelectedDepartment)
                                                                      , General.GetNullableInteger(ViewState["DEPARTMENTTYPE"].ToString())
                                                                      , null
                                                                      , General.GetNullableDateTime(null)
                                                                      , null
                                                                      , General.GetNullableDateTime(null)
                                                                      , General.GetNullableInteger(null)
                                                                      , General.GetNullableInteger(null)
                                                                      , General.GetNullableInteger(null));

        ucStatus.Text = "Action Plan Added.";
        ViewState["ACTIONPLANID"] = actionplanid;
        BindAction();
    }
    private void UpdateCorrectiveAction()
    {

        PhoenixInspectionMOCActionPlan.MOCActionPlanUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                      , General.GetNullableGuid(ViewState["ACTIONPLANID"].ToString())
                                                                      , new Guid((ViewState["MOCID"]).ToString())
                                                                      , int.Parse(ViewState["VESSELID"].ToString())
                                                                      , General.GetNullableGuid(null)
                                                                      , General.GetNullableGuid((ViewState["MOCEXTENTIONID"].ToString()))
                                                                      , txtActionToBeTaken.Text
                                                                      , (ViewState["DEPARTMENTTYPE"].ToString() == "2") ? General.GetNullableInteger(txtPersonOfficeIdEdit.Text) : General.GetNullableInteger(txtCrewIdEdit.Text)
                                                                      , (ViewState["DEPARTMENTTYPE"].ToString() == "2") ? (txtPersonNameEdit.Text) : txtCrewNameEdit.Text
                                                                      , (ViewState["DEPARTMENTTYPE"].ToString() == "2") ? (txtPersonRankEdit.Text) : txtCrewRankEdit.Text
                                                                      , null
                                                                      , DateTime.Parse(txtTargetdateEdit.Text)
                                                                      , General.GetNullableDateTime(null)
                                                                      , General.GetNullableInteger(ucDepartmentedit.SelectedDepartment)
                                                                      , General.GetNullableInteger(ViewState["DEPARTMENTTYPE"].ToString())
                                                                      , null
                                                                      , General.GetNullableDateTime(null)
                                                                      , null
                                                                      , General.GetNullableDateTime(null)
                                                                      , General.GetNullableInteger(null)
                                                                      , General.GetNullableInteger(null)
                                                                      , General.GetNullableInteger(null));
        ucStatus.Text = "MOC Action Plan Updated";
        BindAction();
    }
    private bool IsValidCorrectiveAction()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(txtActionToBeTaken.Text))
            ucError.ErrorMessage = "Corrective Action is required.";

        return (!ucError.IsError);
    }
}