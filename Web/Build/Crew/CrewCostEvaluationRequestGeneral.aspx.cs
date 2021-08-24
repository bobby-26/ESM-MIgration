using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Text;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewCostEvaluationRequestGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            cmdHiddenPick.Attributes.Add("style", "display:none");

            SessionUtil.PageAccessRights(this.ViewState);

            ViewState["REQUESTID"] = null;
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["RECORDSCOUNT"] = "0";
            ViewState["VESSELID"] = null;
            ViewState["EVALUATIONPORTID"] = "";
            ViewState["CURRENTCITYID"] = "";
            ViewState["COMPLETEDYN"] = "";

            if (Request.QueryString["REQUESTID"] != null)
            {
                ViewState["REQUESTID"] = Request.QueryString["REQUESTID"].ToString();
                ViewState["ACTIVE"] = "1";
            }
            if (ViewState["REQUESTID"] != null)
            {
                EditCrewCostEvaluationRequest(new Guid(ViewState["REQUESTID"].ToString()));
                BindAssesment();
            }

            if (Request.QueryString["vslid"] != null)
            {
                ucVessel.SelectedVessel = Request.QueryString["vslid"].ToString();
                ViewState["VESSELID"] = Request.QueryString["vslid"].ToString();
                ucVessel.Enabled = false;
            }
        }
        if (ViewState["REQUESTID"] != null)
        {
            divReqDetail.Visible = true;
            divAssesment.Visible = true;
            ucVessel.Enabled = false;

            EditCrewCostEvaluationRequest(new Guid(ViewState["REQUESTID"].ToString())); 
            SetInformation();
        }
        else
            divReqDetail.Visible = false;

        MainMenu();
        SaveMenu();
        PortMenu();
    }
    private void SetInformation()
    {
        MenuCrewCostGeneralSub.Title = "Request Details(" + PhoenixCrewCostEvaluationRequest.RequestNumber + ")";
    }
    private void SaveMenu()
    {
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuCrewCostGeneralSub.AccessRights = this.ViewState;
        MenuCrewCostGeneralSub.MenuList = toolbarsub.Show();

    }
    private void PortMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        if (ViewState["REQUESTID"] != null)
        {
            toolbar.AddFontAwesomeButton("../Crew/CrewCostEvaluationRequestGeneral.aspx", "Cost Analysis", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewVesselArrivalPortList.aspx?VESSELID=" + ViewState["VESSELID"].ToString() + "&REQUESTID=" + ViewState["REQUESTID"].ToString() + "'); return false;", "Add Port", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
            MenuArraivalPort.AccessRights = this.ViewState;
            MenuArraivalPort.MenuList = toolbar.Show();
        }
    }
    private void MainMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();

        if (ViewState["REQUESTID"] != null)
        {
            toolbar.AddButton("Request", "EVALUATIONREQUEST");
            toolbar.AddButton("Port Cost Details", "REQUESTDETAILS");
            if (Filter.CurrentMenuCodeSelection != "CRW-OPR-CCP")
            {
                toolbar.AddButton("Port Cost Analysis", "ANALYSIS");
            }
            if (Filter.CurrentMenuCodeSelection == "CRW-OPR-CCP")
            {
                toolbar.AddButton("Vessel List", "VESSELLIST");
            }

            MenuCrewCostGeneral.AccessRights = this.ViewState;
            MenuCrewCostGeneral.MenuList = toolbar.Show();
            MenuCrewCostGeneral.SelectedMenuIndex = 1;
        }
        else
        {
            if (Filter.CurrentMenuCodeSelection == "CRW-OPR-CCP")
            {
                toolbar.AddButton("Vessel List", "VESSELLIST");
            }

            toolbar.AddButton("Request", "EVALUATIONREQUEST");
            MenuCrewCostGeneral.AccessRights = this.ViewState;
            MenuCrewCostGeneral.MenuList = toolbar.Show();
        }
    }
    private void BindAssesment()
    {
        if (ViewState["COMPLETEDYN"].ToString() == "1")
            divAssesment.Visible = true;

        DataTable dt = PhoenixCrewCostEvaluationRequest.CrewChangeAssesmentEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , new Guid(ViewState["REQUESTID"].ToString()));

        if (dt.Rows.Count > 0)
        {
            txtCrewChangeCount.Text = dt.Rows[0]["FLDCREWCHANGECOUNT"].ToString();
            ucCrewChangeReason.SelectedReason = dt.Rows[0]["FLDREASONID"].ToString();
        }
    }
    private void EditCrewCostEvaluationRequest(Guid requestid)
    {
        DataTable dt = new DataTable();
        dt = PhoenixCrewCostEvaluationRequest.EditCrewCostEvaluationRequest(requestid);

        if (dt.Rows.Count > 0)
        {
            txtReqNo.Text = dt.Rows[0]["FLDREQUESTNO"].ToString();
            ucVessel.SelectedVessel = dt.Rows[0]["FLDVESSELID"].ToString();
            ViewState["VESSELID"] = dt.Rows[0]["FLDVESSELID"].ToString();
            txtNoofJoiner.Text = dt.Rows[0]["FLDNOOFJOINER"].ToString();
            txtNoofOffSigner.Text = dt.Rows[0]["FLDNOOFOFFSIGNER"].ToString();
            ViewState["COMPLETEDYN"] = dt.Rows[0]["FLDCOMPLETEDYN"].ToString();
            txtNoofJoiner.CssClass = "readonlytextbox";
            txtNoofOffSigner.CssClass = "readonlytextbox";

            txtNoofJoiner.ReadOnly = true;
            txtNoofOffSigner.ReadOnly = true;

            if (General.GetNullableString(txtReqNo.Text.Trim()) != null)
            {
                PhoenixCrewCostEvaluationRequest.RequestNumber = txtReqNo.Text.Trim();
                SetInformation();
            }
        }
    }
    protected void MenuCrewCostGeneralSub_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            Guid? requestid = General.GetNullableGuid(ViewState["REQUESTID"] != null ? ViewState["REQUESTID"].ToString().ToString() : null);

            if (CommandName.ToUpper() == "SAVE")
            {
                if (ViewState["REQUESTID"] == null)
                {
                    if (!IsCrewCostEvaluationRequest())
                    {
                        ucError.Visible = true;
                        return;
                    }
                    else
                    {
                        PhoenixCrewCostEvaluationRequest.InsertCrewCostEvaluationRequest(
                                                General.GetNullableInteger(ucVessel.SelectedVessel)
                                               , General.GetNullableInteger(txtNoofJoiner.Text)
                                               , General.GetNullableInteger(txtNoofOffSigner.Text)
                                               , ref requestid);
                        if (requestid != null)
                        {
                            ViewState["REQUESTID"] = requestid.ToString();
                            EditCrewCostEvaluationRequest(new Guid(ViewState["REQUESTID"].ToString()));
                        }
                    }
                }
                else
                {
                    if (divAssesment.Visible == true)
                    {

                        if (General.GetNullableInteger(ucCrewChangeReason.SelectedReason) == null)
                        {
                            ucError.ErrorMessage = "Reason required";
                            ucError.Visible = true;
                            return;
                        }
                        else
                        {
                            PhoenixCrewCostEvaluationRequest.CrewChangeAssesmentInsertUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                , new Guid(ViewState["REQUESTID"].ToString())
                                 , General.GetNullableInteger(ucCrewChangeReason.SelectedReason));
                            BindAssesment();
                        }

                    }

                }
                MainMenu();
                SaveMenu();
                PortMenu();
                if (ViewState["REQUESTID"] != null)
                {
                    divReqDetail.Visible = true;
                    BindPort();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuCrewCostGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("EVALUATIONREQUEST"))
            {
                Response.Redirect("../Crew/CrewCostEvaluationRequest.aspx", false);
            }
            if (CommandName.ToUpper().Equals("VESSELLIST"))
            {
                Response.Redirect("../Crew/CrewChangePlanFilter.aspx" + (Request.QueryString["access"] != null ? "?access=1" : string.Empty), false);
            }
            if (CommandName.ToUpper().Equals("ANALYSIS"))
            {
                Response.Redirect("../Crew/CrewCostEvaluationFinalPortCostAnalysis.aspx?REQUESTID=" + ViewState["REQUESTID"].ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuArraivalPort_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
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
    private void BindPort()
    {
        try
        {

            DataTable dt = new DataTable();

            dt = PhoenixCrewCostEvaluationRequest.ListCrewCostEvaluationPort(
                General.GetNullableGuid(ViewState["REQUESTID"] != null ? ViewState["REQUESTID"].ToString() : "")
                , General.GetNullableInteger(ViewState["VESSELID"] == null ? "" : ViewState["VESSELID"].ToString()));

            gvEvaluationPort.DataSource = dt;

            if (dt.Rows.Count > 0)
            {
                if (ViewState["EVALUATIONPORTID"].ToString() == "")
                {
                    ViewState["EVALUATIONPORTID"] = dt.Rows[0]["FLDEVALUATIONPORTID"].ToString();
                    ViewState["CURRENTCITYID"] = dt.Rows[0]["FLDCITYID"].ToString();
                }
                ViewState["RECORDSCOUNT"] = "1";
                SetRowSelection();
            }

            MainMenu();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEvaluationPort_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindPort();
    }

    protected void gvEvaluationPort_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {

            LinkButton cme = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cme != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;
            }
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }
        
            RadLabel lblETA = (RadLabel)e.Item.FindControl("lblETA");
            RadLabel lblETD = (RadLabel)e.Item.FindControl("lblETD");

            if (lblETA != null)
            {
                DateTime? dt = General.GetNullableDateTime(lblETA.Text);
                if (dt != null)
                {
                    lblETA.Text = String.Concat(String.Format("{0:dd/MM/yyyy}", dt).ToString() + " " + String.Format("{0:t}", dt).ToString());
                    lblETA.Visible = true;
                }
            }

            if (lblETD != null)
            {
                DateTime? dt = General.GetNullableDateTime(lblETD.Text);
                if (dt != null)
                {
                    lblETD.Text = String.Concat(String.Format("{0:dd/MM/yyyy}", dt).ToString() + " " + String.Format("{0:t}", dt).ToString());
                    lblETD.Visible = true;
                }
            }
            LinkButton cmdVisa = (LinkButton)e.Item.FindControl("cmdVisa");
            RadLabel lblCountry = (RadLabel)e.Item.FindControl("lblCountry");

            if (lblCountry != null && General.GetNullableString(lblCountry.Text) != null)
            {
                if (cmdVisa != null)
                {
                    cmdVisa.Visible = true;
                    cmdVisa.Attributes.Add("onclick", "javascript:openNewWindow('country','','" + Session["sitepath"] + "/Registers/RegistersSeaPortCountryVisa.aspx?countryid=" + lblCountry.Text.Trim() + "'); return false;");
                }
            }

            if (Filter.CurrentMenuCodeSelection == "CRW-OPR-CCP")
            {
                RadLabel lblApprovedYN = (RadLabel)e.Item.FindControl("lblApprovedYN");

                if (lblApprovedYN != null && lblApprovedYN.Text == "1")
                {
                    LinkButton cmdTravel = (LinkButton)e.Item.FindControl("cmdIniTravel");
                    if (cmdTravel != null) cmdTravel.Visible = true;
                }
            }

        }

        if (e.Item.IsInEditMode)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            UserControlMultiColumnCity uccity = (UserControlMultiColumnCity)e.Item.FindControl("txtCityIdEdit");
            if (uccity != null)
            {
                uccity.SelectedValue = drv["FLDCITYID"].ToString();
                uccity.Text = drv["FLDCITYNAME"].ToString();
            }
        }


    }

    protected void gvEvaluationPort_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                RadLabel lblEvaluationPortId = (RadLabel)e.Item.FindControl("lblEvaluationPortId");
                RadLabel lblCityId = (RadLabel)e.Item.FindControl("lblCityId");
                ViewState["EVALUATIONPORTID"] = lblEvaluationPortId.Text;
                ViewState["CURRENTCITYID"] = lblCityId.Text.ToString();
                
                SetRowSelection();

            }
            if (e.CommandName.ToUpper().Equals("CREWCHANGEREQUEST"))
            {
                Response.Redirect("CrewChangeRequest.aspx?REQUESTID=" + ViewState["REQUESTID"].ToString() + "&from=crewcost", false);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEvaluationPort_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string evaluationportid = ((RadLabel)e.Item.FindControl("lblEvaluationPortIdEdit")).Text;
            string cityid = ((UserControlMultiColumnCity)e.Item.FindControl("txtCityIdEdit")).SelectedValue;

            if (General.GetNullableInteger(cityid) == null)
            {
                ucError.ErrorMessage = "City Required";
                ucError.Visible = true;
                return;
            }
            else
            {
                ViewState["CURRENTCITYID"] = cityid;

                PhoenixCrewCostEvaluationRequest.UpdateCrewCostEvaluationPort(new Guid(ViewState["REQUESTID"].ToString()), new Guid(evaluationportid), int.Parse(cityid));
                PhoenixCrewCostEvaluationRequest.UpdateCrewCostAirfareBulk(new Guid(ViewState["REQUESTID"].ToString()), new Guid(evaluationportid), int.Parse(cityid));

                SetRowSelection();
            }

            BindPort();
            gvEvaluationPort.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvEvaluationPort_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            RadLabel lblEvaluationPortId = (RadLabel)e.Item.FindControl("lblEvaluationPortId");

            if (lblEvaluationPortId != null)
            {
                PhoenixCrewCostEvaluationRequest.DeleteCrewCostEvaluationPort(new Guid(lblEvaluationPortId.Text.ToString()));
            }
            BindPort();
            gvEvaluationPort.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    
    protected void ShowExcel()
    {
        try
        {
            PhoenixCrew2XL.Export2XLCrewChangeCostComparision(new Guid(ViewState["REQUESTID"].ToString()));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetRowSelection()
    {
        try
        {
          
            foreach (GridDataItem item in gvEvaluationPort.Items)
            {
                if (item.GetDataKeyValue("FLDEVALUATIONPORTID").ToString() == ViewState["EVALUATIONPORTID"].ToString())
                {
                    gvEvaluationPort.SelectedIndexes.Clear();

                    gvEvaluationPort.SelectedIndexes.Add(item.ItemIndex);

                }
            }

            if (ViewState["REQUESTID"] != null)
            {
                ifMoreInfo.Attributes["src"] = "../Crew/CrewCostEvaluationQuote.aspx?REQUESTID=" + ViewState["REQUESTID"].ToString() + "&EVALUATIONPORTID=" + ViewState["EVALUATIONPORTID"].ToString() + "&CURRENTCITYID=" + ViewState["CURRENTCITYID"].ToString();
            }
        }

        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
            return;
        }
    }

    private bool IsCrewCostEvaluationRequest()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
        {
            ucError.ErrorMessage = "Vessel required";
        }

        if (General.GetNullableInteger(txtNoofJoiner.Text) == null)
        {
            ucError.ErrorMessage = "Joiners Required";
        }
        if (General.GetNullableInteger(txtNoofOffSigner.Text) == null)
        {
            ucError.ErrorMessage = "OffSigners required";
        }
        return (!ucError.IsError);
    }
  
    protected void cmdHiddenSubmit_Click(object sender, EventArgs args)
    {

    }
    protected void cmdHiddenPick_Click(object sender, EventArgs args)
    {

    }


}
