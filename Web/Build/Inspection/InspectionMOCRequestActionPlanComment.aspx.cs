using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using System.Collections;
public partial class InspectionMOCRequestActionPlanComment : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);
            MenuMOCPlanComment.AccessRights = this.ViewState;
            MenuMOCPlanComment.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["MOCID"] = null;
                ViewState["ACTIONPLANID"] = "";
                ViewState["DEPARTMENTID"] = "";

                if (Request.QueryString["MOCID"] != null && Request.QueryString["MOCID"].ToString() != string.Empty)
                    ViewState["MOCID"] = Request.QueryString["MOCID"].ToString();

                if (Request.QueryString["VESSELID"] != null && Request.QueryString["VESSELID"].ToString() != string.Empty)
                    ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
            }
            BindCrewList();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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

    protected void MenuMOCPlanComment_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Inspection/InspectionMOCRequestActionPlan.aspx?&VESSELID=" + ViewState["VESSELID"].ToString() + "&MOCID=" + ViewState["MOCID"].ToString());
            }
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (InspectionFilter.CurrentMOCActionPlanComment != null)
                {
                    ArrayList selectedcomments = (ArrayList)InspectionFilter.CurrentMOCActionPlanComment;
                    if (selectedcomments != null && selectedcomments.Count > 0)
                    {
                        foreach (Guid commentid in selectedcomments)
                        {
                            PhoenixInspectionMOCActionPlan.MOCActionPlanEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                          , new Guid(commentid.ToString())
                                                                                          , new Guid((ViewState["MOCID"]).ToString())
                                                                                          , int.Parse(ViewState["VESSELID"].ToString())
                                                                                          , (ViewState["DEPARTMENTTYPE"].ToString() == "2") ? General.GetNullableInteger(txtPersonOfficeIdEdit.Text) : General.GetNullableInteger(txtCrewIdEdit.Text)
                                                                                          , (ViewState["DEPARTMENTTYPE"].ToString() == "2") ? (txtPersonNameEdit.Text) : txtCrewNameEdit.Text
                                                                                          , (ViewState["DEPARTMENTTYPE"].ToString() == "2") ? (txtPersonRankEdit.Text) : txtCrewRankEdit.Text
                                                                                          , DateTime.Parse(txtTargetdateEdit.Text)
                                                                                        );

                        }
                    }                   
                }
                InspectionFilter.CurrentMOCActionPlanComment = null;
                ucStatus.Text = "Office comments updated for selected comments.";
                Response.Redirect("../Inspection/InspectionMOCRequestActionPlan.aspx?&VESSELID=" + ViewState["VESSELID"].ToString() + "&MOCID=" + ViewState["MOCID"].ToString());
            }           
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidCorrectiveAction()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        return (!ucError.IsError);
    }

}
