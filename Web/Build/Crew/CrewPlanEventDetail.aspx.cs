using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;
using System.Web.UI;
using System.Collections.Specialized;
using System.Xml;
using System.IO;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewOffshore;
using System.Web.UI.HtmlControls;

public partial class CrewPlanEventDetail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Event List", "LIST");
            toolbar.AddButton("Event Detail", "DETAIL");
            toolbar.AddButton("Relief Plan", "RELIEFPLAN");
            toolbar.AddButton("Crew List", "CREWLIST");
            toolbar.AddButton("Crew Visual", "VISUAL");
            CrewMenu.AccessRights = this.ViewState;
            CrewMenu.MenuList = toolbar.Show();
            CrewMenu.SelectedMenuIndex = 1;

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddButton("Save", "SAVE", ToolBarDirection.Right);
            CrewTab.AccessRights = this.ViewState;
            CrewTab.MenuList = toolbar1.Show();

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewPlanEventDetail.aspx", "Send Details", "<i class=\"fas fa-envelope\"></i>", "EMAIL");
            //toolbargrid.AddFontAwesomeButton("../Crew/CrewPlanEventDetail.aspx", "Add Crew", "<i class=\"fa fa-plus-circle\"></i>", "ADDPLAN");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewPlanEventDetail.aspx", "Off-Signer Request", "<i class=\"fas fa-plane-arrival\"></i>", "OFFSIGNERTRAVEL");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewPlanEventDetail.aspx", "On-Signer Request", "<i class=\"fas fa-plane-departure-On-Signer-Request\"></i>", "ONSIGNERTRAVEL");
            toolbargrid.AddImageButton("../Crew/CrewPlanEventDetail.aspx", "Generate CrewDetails xml", "annexure.png", "GENERATEXML");
            gvEventTab.AccessRights = this.ViewState;
            gvEventTab.MenuList = toolbargrid.Show();
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                ViewState["eventid"] = null;
                ViewState["VESSELID"] = "";
                if (Request.QueryString["eventid"] != null && Request.QueryString["eventid"].ToString() != "")
                {
                    ViewState["eventid"] = Request.QueryString["eventid"].ToString();
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                SetEventDetail();
                gvEvent.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetEventDetail()
    {
        try
        {
            DataTable dt = PhoenixCrewChangeEvent.EditCrewChangeEvent(new Guid(ViewState["eventid"].ToString()));
            if (dt.Rows.Count > 0)
            {
                ViewState["SEAPORTAIRPORTID"] = "";
                ViewState["SEAPORTAIRPORTCITYID"] = "";
                ViewState["SEAPORTAIRPORTCITYNAME"] = "";

                txtVessel.Text = dt.Rows[0]["FLDVESSELNAME"].ToString();
                txtEventDate.Text = dt.Rows[0]["FLDEVENTDATE"].ToString();
                txtEventToDate.Text = dt.Rows[0]["FLDEVENTTODATE"].ToString();
                ucPortAgent.SelectedValue = dt.Rows[0]["FLDPORTAGENTID"].ToString();
                ucPortAgent.Text = dt.Rows[0]["FLDPORTAGENTNAME"].ToString();
                ucPort.SelectedValue = dt.Rows[0]["FLDPORTID"].ToString();
                txtRefNo.Text = dt.Rows[0]["FLDREFERENCENO"].ToString();

                txtETB.Text = dt.Rows[0]["FLDETB"].ToString();
                txtETC.Text = dt.Rows[0]["FLDETC"].ToString();                
                ucTravelAgent.SelectedValue = dt.Rows[0]["FLDTRAVELAGENTID"].ToString();
                ucTravelAgent.Text = dt.Rows[0]["FLDTRAVELAGENTNAME"].ToString();
                ucSubAgent.SelectedValue = dt.Rows[0]["FLDSUBAGENTID"].ToString();
                ucSubAgent.Text = dt.Rows[0]["FLDSUBAGENTNAME"].ToString();
                txtCost.Text = dt.Rows[0]["FLDCOST"].ToString();

                if (ucPort.SelectedValue != null && ucPort.SelectedValue != "")
                {
                    DataTable dtport = PhoenixRegistersSeaport.EditSeaport(General.GetNullableInteger(ucPort.SelectedValue));

                    if (dtport.Rows.Count > 0)
                    {
                        ViewState["SEAPORTAIRPORTID"] = dtport.Rows[0]["FLDAIRPORTID"].ToString();
                        ViewState["SEAPORTAIRPORTCITYID"] = dtport.Rows[0]["FLDCITYID"].ToString();
                        ViewState["SEAPORTAIRPORTCITYNAME"] = dtport.Rows[0]["FLDCITYNAME"].ToString();
                    }
                }

                ucPort.Text = dt.Rows[0]["FLDPORTNAME"].ToString();
                txtVesselArrival.Text = dt.Rows[0]["FLDVESSELARRIVAL"].ToString();
                txtVesselDepature.Text = dt.Rows[0]["FLDVESSELDEPARTURE"].ToString();
                ddlStatus.SelectedValue = dt.Rows[0]["FLDSTATUS"].ToString();
                txtRemarks.Text = dt.Rows[0]["FLDREMARKS"].ToString();
                ViewState["VESSELID"] = dt.Rows[0]["FLDVESSELID"].ToString();

                BindVesselAccount();
                foreach (RadComboBoxItem item in ddlAccountDetails.Items)
                {
                    if (item.Value == dt.Rows[0]["FLDVESSELACCOUNTID"].ToString())
                    {
                        item.Selected = true;
                        break;
                    }
                }
                if (dt.Rows[0]["FLDAIRPORTID"].ToString() != null && dt.Rows[0]["FLDAIRPORTID"].ToString() != "")
                {
                    ucAirport.SelectedAirport = dt.Rows[0]["FLDAIRPORTID"].ToString();
                }
                else
                {
                    ucAirport.SelectedAirport = ViewState["SEAPORTAIRPORTID"].ToString();
                }
                if (dt.Rows[0]["FLDAIRPORTCITY"].ToString() != null && dt.Rows[0]["FLDAIRPORTCITY"].ToString() != "")
                {
                    ucMultiCity.SelectedValue = dt.Rows[0]["FLDAIRPORTCITY"].ToString();
                    ucMultiCity.Text = dt.Rows[0]["FLDCITYNAME"].ToString();
                }
                else
                {
                    ucMultiCity.SelectedValue = ViewState["SEAPORTAIRPORTCITYID"].ToString();
                    ucMultiCity.Text = ViewState["SEAPORTAIRPORTCITYNAME"].ToString();
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void CrewMenu_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../Crew/CrewPlanEvent.aspx", false);
            }
            if (CommandName.ToUpper().Equals("DETAIL"))
            {

            }
            if (CommandName.ToUpper().Equals("CREWLIST"))
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
                {

                    String scriptpopup = String.Format("javascript:openNewWindow('codehelp2','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreCrewList.aspx?vesselid=" + ViewState["VESSELID"].ToString() + "');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", scriptpopup, true);

                }
                else
                {

                    String scriptpopup = String.Format("javascript:openNewWindow('codehelp2','','" + Session["sitepath"] + "/Crew/CrewList.aspx?vesselid=" + ViewState["VESSELID"].ToString() + "');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", scriptpopup, true);
                }
            }
            if (CommandName.ToUpper().Equals("RELIEFPLAN"))
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
                {

                    String scriptpopup = String.Format("javascript:openNewWindow('codehelp2','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreReliefPlan.aspx?pl=6&launchedfrom=offshore&vesselid=" + ViewState["VESSELID"].ToString() + "');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", scriptpopup, true);

                }
                else
                {

                    String scriptpopup = String.Format("javascript:openNewWindow('codehelp2','','" + Session["sitepath"] + "/Crew/CrewList.aspx?vesselid=" + ViewState["VESSELID"].ToString() + "');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", scriptpopup, true);
                }
            }

            if (CommandName.ToUpper().Equals("VISUAL"))
            {

                String scriptpopup = String.Format("javascript:openNewWindow('codehelp2','','" + Session["sitepath"] + "/Crew/CrewEmployeeSignOnOffGanttChart.aspx?VesselID=" + ViewState["VESSELID"] + "',false,900,350);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", scriptpopup, true);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindVesselAccount()
    {
        ddlAccountDetails.SelectedValue = "";
        ddlAccountDetails.Text = "";

        string vesselid = (ViewState["VESSELID"] == null) ? null : (ViewState["VESSELID"].ToString());

        DataSet dsl = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(
            General.GetNullableInteger(vesselid) == 0 ? null : General.GetNullableInteger(vesselid), 1);
        ddlAccountDetails.DataSource = dsl;
        ddlAccountDetails.DataBind();

        //if (dsl.Tables[0].Rows.Count == 1)
        //{
        //    ddlAccountDetails.SelectedValue = "";
        //    ddlAccountDetails.Text = "";
        //    ddlAccountDetails.SelectedValue = dsl.Tables[0].Rows[0]["FLDACCOUNTID"].ToString();
        //}

    }
    protected void ddlAccountDetails_DataBound(object sender, EventArgs e)
    {
        ddlAccountDetails.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }

    protected void CrewTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixCrewChangeEvent.UpdateCrewPlanEvent(new Guid(ViewState["eventid"].ToString())
                                , int.Parse(ViewState["VESSELID"].ToString())
                                , General.GetNullableDateTime(txtEventDate.Text)
                                , General.GetNullableDateTime(txtEventToDate.Text)
                                , General.GetNullableInteger(ucPort.SelectedValue)
                                , General.GetNullableInteger(ucPortAgent.SelectedValue)
                                , General.GetNullableDateTime(txtVesselArrival.Text)
                                , General.GetNullableDateTime(txtVesselDepature.Text)
                                , General.GetNullableInteger(ddlStatus.SelectedValue)
                                , General.GetNullableInteger(ddlAccountDetails.SelectedValue)
                                , General.GetNullableInteger(ucAirport.SelectedAirport)
                                , General.GetNullableInteger(ucMultiCity.SelectedValue)
                                , General.GetNullableDateTime(txtETB.Text)
                                , General.GetNullableDateTime(txtETC.Text)
                                , General.GetNullableInteger(ucTravelAgent.SelectedValue)
                                , General.GetNullableInteger(ucSubAgent.SelectedValue)
                                , General.GetNullableDecimal(txtCost.Text)
                                );
                ucStatus.Text = "Information Updated";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void chkAllOffSigner_CheckedChanged(object sender, EventArgs e)
    {
        RadCheckBox headerCheckBox = (sender as RadCheckBox);
        foreach (GridDataItem dataItem in gvEvent.MasterTableView.Items)
        {
            if ((dataItem.FindControl("chkOffSigner") as RadCheckBox).Enabled == true)
            {
                (dataItem.FindControl("chkOffSigner") as RadCheckBox).Checked = headerCheckBox.Checked;
                dataItem.Selected = headerCheckBox.Checked == true;
            }
        }
    }


    protected void chkAllOnSigner_CheckedChanged(object sender, EventArgs e)
    {
        RadCheckBox headerCheckBox = (sender as RadCheckBox);
        foreach (GridDataItem dataItem in gvEvent.MasterTableView.Items)
        {
            (dataItem.FindControl("chkOnSigner") as RadCheckBox).Checked = headerCheckBox.Checked;
            dataItem.Selected = headerCheckBox.Checked == true;
        }
    }

    protected void gvEventTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("EMAIL"))
            {
                String scriptpopup = String.Format("javascript:openNewWindow('codehelp2','','" + Session["sitepath"] + "/Crew/CrewPlanEventEmail.aspx?VESSELID=" + ViewState["VESSELID"].ToString() + "&eventid=" + ViewState["eventid"].ToString() + "');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", scriptpopup, true);

            }
            //if (CommandName.ToUpper() == "ADDPLAN")
            //{

            //    String scriptpopup = String.Format("javascript:openNewWindow('codehelp2','','" + Session["sitepath"] + "/Crew/CrewPlanEventCrew.aspx?VESSELID=" + ViewState["VESSELID"].ToString() + "&eventid=" + ViewState["eventid"].ToString() + "');");
            //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", scriptpopup, true);

            //}
            if (CommandName.ToUpper() == "OFFSIGNERTRAVEL")
            {

                string csvCrewPlanList = GetSelectedOffSigners();

                if (!isValidMapEvent(csvCrewPlanList))
                {
                    ucError.Visible = true;
                    return;
                }

                Filter.CurrentCrewPlanEventTravelReqFilter = null;

                NameValueCollection criteria = new NameValueCollection();

                criteria.Add("EventDetailList", csvCrewPlanList);

                Filter.CurrentCrewPlanEventTravelReqFilter = criteria;

                String scriptpopup = String.Format("javascript:openNewWindow('codehelp2','','" + Session["sitepath"] + "/Crew/CrewPlanDetailOffSignerTravel.aspx?creweventid=" + ViewState["eventid"].ToString() + "',false,1000,500);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", scriptpopup, true);

            }

            if (CommandName.ToUpper() == "ONSIGNERTRAVEL")
            {

                string csvCrewPlanList = GetSelectedOnSigners();

                if (!isValidMapEvent(csvCrewPlanList))
                {
                    ucError.Visible = true;
                    return;
                }

                Filter.CurrentCrewPlanEventTravelReqFilter = null;

                NameValueCollection criteria = new NameValueCollection();

                criteria.Add("EventDetailList", csvCrewPlanList);

                Filter.CurrentCrewPlanEventTravelReqFilter = criteria;

                String scriptpopup = String.Format("javascript:openNewWindow('codehelp2','','" + Session["sitepath"] + "/Crew/CrewPlanDetailTravel.aspx?creweventid=" + ViewState["eventid"].ToString() + "',false,1000,500);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", scriptpopup, true);

            }
            else if (CommandName.ToUpper().Equals("GENERATEXML"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                string Vesselid = (ViewState["Vesselid"] == null) ? null : (ViewState["Vesselid"].ToString());
                int? sortdirection = null;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                DataSet ds = PhoenixCrewChangeEventDetail.CrewEventDetailSearch(General.GetNullableGuid(ViewState["eventid"].ToString())
                                                                             , sortexpression, sortdirection
                                                                             , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                             , gvEvent.PageSize
                                                                             , ref iRowCount
                                                                             , ref iTotalPageCount);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string path = Server.MapPath("~/Attachments/Crew/" + ViewState["eventid"].ToString() + "_" + txtVessel.Text + "_" + "CrewDetails.xml");
                    try
                    {
                        string xml = "";
                        DataSet dssummary = PhoenixCrewChangeEventDetail.CrewDetailsXML(new Guid(ViewState["eventid"].ToString()));
                        XmlDocument doc = new XmlDocument();
                        for (int i = 0; i < dssummary.Tables[0].Rows.Count; i++)
                        {
                            string value = dssummary.Tables[0].Rows[i][0].ToString();
                            xml = xml + value;
                        }

                        doc.LoadXml(xml);
                        // Save the document to a file and auto-indent the output.
                        if (File.Exists(path))
                            File.Delete(path);
                        using (XmlTextWriter writer = new XmlTextWriter(path, null))
                        {
                            writer.Formatting = Formatting.Indented;
                            doc.Save(writer);
                        }
                        string attachment = "attachment; filename=CrewDetails.xml";
                        Response.ClearContent();
                        Response.ContentType = "application/xml";
                        Response.AddHeader("content-disposition", attachment);
                        Response.Write(xml);
                        Response.End();
                    }
                    catch (Exception ex)
                    {
                        ucError.ErrorMessage = ex.Message;
                        ucError.Visible = true;
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
    protected bool isValidMapEvent(string csvCrewPlanList)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(csvCrewPlanList) == null)
            ucError.ErrorMessage = "Please select any seafarers";

        return (!ucError.IsError);
    }

    private string GetSelectedOffSigners()
    {
        StringBuilder strlist = new StringBuilder();

        if (gvEvent.MasterTableView.Items.Count > 0)
        {
            foreach (GridDataItem gvr in gvEvent.MasterTableView.Items)
            {
                RadCheckBox chkAdd = (RadCheckBox)gvr.FindControl("chkOffSigner");

                if (chkAdd.Checked == true)
                {
                    RadLabel lblCrewEventDetailId = (RadLabel)gvr.FindControl("lblCrewEventDetailId");

                    strlist.Append(lblCrewEventDetailId.Text);
                    strlist.Append(",");
                }
            }

            if (strlist.Length > 1)
            {
                strlist.Remove(strlist.Length - 1, 1);
            }

        }

        return strlist.ToString();


    }

    private string GetSelectedOnSigners()
    {
        StringBuilder strlist = new StringBuilder();

        if (gvEvent.MasterTableView.Items.Count > 0)
        {
            foreach (GridDataItem gvr in gvEvent.MasterTableView.Items)
            {
                RadCheckBox chkAdd = (RadCheckBox)gvr.FindControl("chkOnSigner");

                if (chkAdd.Checked == true)
                {
                    RadLabel lblCrewEventDetailId = (RadLabel)gvr.FindControl("lblCrewEventDetailId");

                    strlist.Append(lblCrewEventDetailId.Text);
                    strlist.Append(",");
                }
            }

            if (strlist.Length > 1)
            {
                strlist.Remove(strlist.Length - 1, 1);
            }

        }

        return strlist.ToString();


    }
    protected void gvEvent_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvEvent.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            string Vesselid = (ViewState["Vesselid"] == null) ? null : (ViewState["Vesselid"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewChangeEventDetail.CrewEventDetailSearch(General.GetNullableGuid(ViewState["eventid"].ToString())
                                                                         , sortexpression, sortdirection
                                                                         , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                         , gvEvent.PageSize
                                                                         , ref iRowCount
                                                                         , ref iTotalPageCount);


            gvEvent.DataSource = ds;
            gvEvent.VirtualItemCount = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEvent_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "REMOVEPLAN")
            {
                string lblEventDetailId = ((RadLabel)e.Item.FindControl("lblCrewEventDetailId")).Text;

                if (lblEventDetailId != "" && lblEventDetailId != null)
                {
                    PhoenixCrewChangeEventDetail.DeleteCrewPlanEvent(new Guid(lblEventDetailId));
                }

                BindData();
                gvEvent.Rebind();
            }
            if (e.CommandName.ToUpper() == "ADDONSIGNER")
            {
                String scriptpopup = String.Format("javascript:openNewWindow('codehelp2','','" + Session["sitepath"] + "/Crew/CrewPlanEventCrew.aspx?VESSELID=" + ViewState["VESSELID"].ToString() + "&eventid=" + ViewState["eventid"].ToString() + "');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", scriptpopup, true);
            }
            if (e.CommandName.ToUpper() == "ADDOFFSIGNER")
            {
                String scriptpopup = String.Format("javascript:openNewWindow('codehelp2','','" + Session["sitepath"] + "/Crew/CrewPlanEventOnboardCrew.aspx?VESSELID=" + ViewState["VESSELID"].ToString() + "&eventid=" + ViewState["eventid"].ToString() + "');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", scriptpopup, true);
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            else if (e.CommandName.ToUpper() == "DEPLAN")
            {
                try
                {


                    string crewPlanId = ((RadLabel)e.Item.FindControl("lblCrewPlanId")).Text;
                    string empid = ((RadLabel)e.Item.FindControl("lblEmployeeId")).Text;
                    PhoenixCrewPlanning.DeleteCrewPlan(new Guid(crewPlanId), int.Parse(empid));
                    BindData();
                    gvEvent.Rebind();

                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;

                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEvent_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            RadLabel lblOffSigner = (RadLabel)e.Item.FindControl("lblOffSignerId");
            RadLabel lbloffsignercon = (RadLabel)e.Item.FindControl("lbloffsignercon");
            RadCheckBox chkOffSigner = (RadCheckBox)e.Item.FindControl("chkOffSigner");

            RadLabel lblOnSigner = (RadLabel)e.Item.FindControl("lblEmployeeId");
            RadCheckBox chkOnSigner = (RadCheckBox)e.Item.FindControl("chkOnSigner");

            RadLabel lblOnSignerTravelReq = (RadLabel)e.Item.FindControl("lblOnSignerTravelReq");
            RadLabel lblOffSignerTravelReq = (RadLabel)e.Item.FindControl("lblOffSignerTravelReq");

            RadLabel lblCrewEventDetailId = (RadLabel)e.Item.FindControl("lblCrewEventDetailId");
            LinkButton cmdOnSignerTravel = (LinkButton)e.Item.FindControl("cmdOnSignerTravel");
            LinkButton cmdOffSignerTravel = (LinkButton)e.Item.FindControl("cmdOffSignerTravel");

            RadLabel lblCrewPlanId = (RadLabel)e.Item.FindControl("lblCrewPlanId");
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkRelievee");
            RadLabel lblPDStatusID = (RadLabel)e.Item.FindControl("lblPDStatusID");
            LinkButton co = (LinkButton)e.Item.FindControl("cmdCourse");
            LinkButton pd = (LinkButton)e.Item.FindControl("cmdPDForm");
            UserControlCommonToolTip ucCommonToolTip = (UserControlCommonToolTip)e.Item.FindControl("ucCommonToolTip");
            RadLabel VesselId = (RadLabel)e.Item.FindControl("lblVesselId");
            RadLabel rank = (RadLabel)e.Item.FindControl("lblRankId");
            LinkButton sho = (LinkButton)e.Item.FindControl("cmdShow");
            LinkButton cmdAppointmentLetter = (LinkButton)e.Item.FindControl("cmdAppointmentLetter");
            LinkButton cmdAppLetter = (LinkButton)e.Item.FindControl("cmdAppLetter");
            LinkButton cmdCancelAppointment = (LinkButton)e.Item.FindControl("cmdCancelAppointment");
            LinkButton md = (LinkButton)e.Item.FindControl("cmdMedical");
            LinkButton ccmdDocChecklist = (LinkButton)e.Item.FindControl("cmdDocChecklist");
            LinkButton cmdOfferLetter = (LinkButton)e.Item.FindControl("cmdOfferLetter");
            LinkButton cmdApproveSignOn = (LinkButton)e.Item.FindControl("cmdApproveSignOn");
            LinkButton cmdApproveTravel = (LinkButton)e.Item.FindControl("cmdApproveTravel");
            RadLabel joindate = (RadLabel)e.Item.FindControl("lblJoinDate");
            LinkButton cmdIniTravel = (LinkButton)e.Item.FindControl("cmdIniTravel");
            LinkButton cmdRemark = (LinkButton)e.Item.FindControl("cmdRemark");

            string newapplicant = "";
            if (drv["FLDNEWAPP"].ToString() == "1")
            {
                newapplicant = "1";
            }
            string personalmaster = "";

            if (newapplicant == "1")
            {
                personalmaster = "";
            }
            else
            {
                personalmaster = "1";
                newapplicant = "";
            }
            if (lblOffSigner.Text.Trim() == "-1")
            {
                chkOffSigner.Enabled = false;
                lbloffsignercon.Visible = false;
            }
            if (lblOnSigner.Text.Trim() == "-1")
            {
                chkOnSigner.Enabled = false;
            }

            if (lblOnSignerTravelReq.Text.Trim() != "1")
            {
                cmdOnSignerTravel.Enabled = false;
                cmdOnSignerTravel.Visible = false;
            }
            if (lblOffSignerTravelReq.Text.Trim() != "1")
            {
                cmdOffSignerTravel.Enabled = false;
                cmdOffSignerTravel.Visible = false;
            }


            if (cmdOnSignerTravel != null)
            {
                cmdOnSignerTravel.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPlanEventTravelDetails.aspx?onsigneryn=1&eventdetailid=" + lblCrewEventDetailId.Text + "'); return false;");
            }
            if (cmdOffSignerTravel != null)
            {
                cmdOffSignerTravel.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPlanEventTravelDetails.aspx?onsigneryn=0&eventdetailid=" + lblCrewEventDetailId.Text + "'); return false;");
            }

            LinkButton deletedetailplan = (LinkButton)e.Item.FindControl("cmdRemovePlan");
            if (deletedetailplan != null)
            {
                deletedetailplan.Visible = SessionUtil.CanAccess(this.ViewState, deletedetailplan.CommandName);
                deletedetailplan.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event,'Are you sure you want to remove this plan from this event?')");
            }


            LinkButton lnkOnSigner = (LinkButton)e.Item.FindControl("lnkOnSigner");
            LinkButton lnkOffSigner = (LinkButton)e.Item.FindControl("lnkOffSigner");


            if (drv["FLDONSIGNERID"].ToString() != string.Empty && lnkOnSigner != null)
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
                {
                    if (drv["FLDONSIGNERNEWAPPYN"].ToString() == "1")
                    {
                        lnkOnSigner.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreNewApplicantPersonalGeneral.aspx?empid=" + drv["FLDONSIGNERID"].ToString() + "'); return false;");
                    }
                    else
                    {
                        lnkOnSigner.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + drv["FLDONSIGNERID"].ToString() + "'); return false;");
                    }

                }
                else
                {
                    if (drv["FLDONSIGNERNEWAPPYN"].ToString() == "1")
                    {
                        lnkOnSigner.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewNewApplicantPersonalGeneral.aspx?empid=" + drv["FLDONSIGNERID"].ToString() + "'); return false;");
                    }
                    else
                    {
                        lnkOnSigner.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + drv["FLDONSIGNERID"].ToString() + "'); return false;");
                    }
                }
            }

            if (drv["FLDOFFSIGNERID"].ToString() != string.Empty && drv["FLDOFFSIGNERID"].ToString() != "-1" && lnkOffSigner != null)
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
                {
                    lnkOffSigner.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + drv["FLDOFFSIGNERID"].ToString() + "'); return false;");
                }
                else
                {
                    lnkOffSigner.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + drv["FLDOFFSIGNERID"].ToString() + "'); return false;");
                }
            }
            //relief plan icons

            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit)
            && !e.Item.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event,'Are you sure you want to deplan the seafarer and remove from event ?')");
                    if (lblCrewPlanId.Text == string.Empty) { db.Visible = false; eb.Visible = false; } else { foreach (TableCell c in e.Item.Cells) c.CssClass = "bluefont"; }
                }
                else
                {
                    e.Item.Attributes["onclick"] = "";
                }

                if (lbr != null) lbr.Attributes.Add("onclick", "javascript:parent.openNewWindow('CrewPage','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + lblOffSigner.Text + "&launchedfrom=offshore'); return true;");

                LinkButton lbrv = (LinkButton)e.Item.FindControl("lnkReliever");
                if (lbrv != null) lbrv.Attributes.Add("onclick", "javascript:parent.openNewWindow('CrewPage','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + lblOffSigner.Text + "&launchedfrom=offshore" + "&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "'); return true;");

                if (lblOffSigner != null && !string.IsNullOrEmpty(lblOffSigner.Text))
                {
                    if (lblPDStatusID != null && lblPDStatusID.Text != PhoenixCommonRegisters.GetHardCode(1, 99, "AWA") && lblPDStatusID.Text != PhoenixCommonRegisters.GetHardCode(1, 99, "APR")) // Proposed,Approval rejected
                    {
                        if (co != null)
                        {
                            if (Request.QueryString["launchedfrom"] != null && Request.QueryString["launchedfrom"].ToString() != null)
                            {
                                co.Attributes.Add("onclick", "javascript:parent.openNewWindow('chml', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreCourseMissing.aspx?empid=" + lblOnSigner.Text + "&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "');return false;");
                                co.Visible = true;
                            }
                            else
                            {
                                co.Attributes.Add("onclick", "javascript:openNewWindow('chml', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreCourseMissing.aspx?empid=" + lblOnSigner.Text + "&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "');return false;");
                                co.Visible = true;
                            }
                        }
                    }
                    else
                        if (co != null) co.Visible = false;
                }
                if (lblOffSigner != null && !string.IsNullOrEmpty(lblOffSigner.Text))
                {
                    if (pd != null) pd.Attributes.Add("onclick", "parent.openNewWindow('CrewPage', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=EMPLOYEECV&employeeid=" + lblOnSigner.Text + "&vslid=" + VesselId.Text + "&rankid=" + rank.Text + "');return false;");
                    if (ucCommonToolTip != null) ucCommonToolTip.Visible = true;
                }
                else
                {
                    if (pd != null) pd.Visible = false;
                    if (ucCommonToolTip != null) ucCommonToolTip.Visible = false;

                }

                DataTable dt = new DataTable();
                if (lblCrewPlanId != null) dt = PhoenixCrewOffshoreCrewChange.WaivedDocumentList(General.GetNullableGuid(lblCrewPlanId.Text));
                if (dt.Rows.Count == 0)
                {
                    if (ucCommonToolTip != null)
                        ucCommonToolTip.Visible = false;
                }
                else
                {
                    if (ucCommonToolTip != null)
                        ucCommonToolTip.Visible = true;
                }
                if (sho != null)
                {
                    if ((lblOffSigner != null && lblOffSigner.Text == string.Empty && sho != null) || lblOffSigner.Text == "-1") sho.Visible = false;
                    if (lblOnSigner != null && lblOnSigner.Text == string.Empty && cmdAppointmentLetter != null) cmdAppointmentLetter.Visible = false;
                }
                if (lblCrewPlanId != null && lblCrewPlanId.Text == "")
                {
                    db.Visible = false;
                }

                if (cmdAppLetter != null && lblOffSigner != null)
                {
                    cmdAppLetter.Attributes.Add("onclick", "parent.openNewWindow('chml', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreAppointmentLetter.aspx?employeeid="
                        + lblOffSigner.Text
                        + "&crewplanid=" + drv["FLDCREWPLANID"].ToString()
                        + "&appointmentletterid=" + drv["FLDAPPOINTMENTLETTERID"].ToString()
                        + "&popup=1" + "');return false;");
                }
                if (cmdCancelAppointment != null && lblOffSigner != null)
                {
                    cmdCancelAppointment.Attributes.Add("onclick", "javascript:parent.openNewWindow('CAPP','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreCancelAppointmentReasonUpdate.aspx?CREWPLANID=" + drv["FLDCREWPLANID"].ToString()
                                + "&APPOINTMENTLETTERID=" + drv["FLDAPPOINTMENTLETTERID"].ToString()
                                + "&EMPLOYEEID=" + lblOffSigner.Text + "','medium'); return true;");
                }
                if (cmdAppointmentLetter != null)
                {
                    cmdAppointmentLetter.Attributes.Add("onclick", "parent.openNewWindow('chml', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=11&reportcode=APPOINMENTLETTERPDF&showmenu=0&showword=0&showexcel=0"
                        + "&crewplanid=" + drv["FLDCREWPLANID"].ToString()
                        + "&appointmentletterid=" + drv["FLDAPPOINTMENTLETTERID"].ToString()
                        + "&popup=1" + "');return false;");
                    //cmdAppointmentLetter.Attributes.Add("onclick", "parent.Openpopup('chml', '', '../Reports/ReportsViewWithSubReport.aspx?applicationcode=11&reportcode=APPOINMENTLETTER&showmenu=0&showword=0&showexcel=0"
                    //    + "&crewplanid=554AA460-ECD1-E411-A56C-D8D385A9EF98"
                    //    + "&appointmentletterid=85DB2437-EDD1-E411-A56C-D8D385A9EF98"
                    //    + "&popup=1" + "');return false;");
                }
                if (lblCrewPlanId != null && lblCrewPlanId.Text != "" && md != null && lblOffSigner != null) // crew change plan buttons
                {
                    if (!string.IsNullOrEmpty(lblOffSigner.Text))
                    {
                        if (lblPDStatusID != null && lblPDStatusID.Text != PhoenixCommonRegisters.GetHardCode(1, 99, "AWA") && lblPDStatusID.Text != PhoenixCommonRegisters.GetHardCode(1, 99, "APR")) // Proposed, Approval rejected
                        {
                            if (md != null && lblOffSigner != null) md.Attributes.Add("onclick", "parent.openNewWindow('chml', '', '" + Session["sitepath"] + "/Crew/CrewMedicalSlip.aspx?empid=" + lblOnSigner.Text + "&vslid=" + VesselId.Text + "');return false;");
                            if (md != null) md.Visible = true;
                        }
                        else
                        {
                            if (md != null) md.Visible = false;
                        }
                    }
                    else
                        if (md != null) md.Visible = false;



                    if (lblPDStatusID != null && lblPDStatusID.Text == PhoenixCommonRegisters.GetHardCode(1, 99, "APV")) // Approval for vessel
                    {
                        if (ccmdDocChecklist != null)
                        {
                            ccmdDocChecklist.Visible = true;
                            ccmdDocChecklist.Attributes.Add("onclick", "javascript:parent.openNewWindow('CAPP','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreOfficeDocumentChecklist.aspx?Crewplanid=" + drv["FLDCREWPLANID"].ToString() + "'); return true;");
                        }
                        LinkButton email = (LinkButton)e.Item.FindControl("cmdDocumentCheckLIstmail");
                        if (email != null)
                        {
                            email.Visible = true;
                            email.Visible = SessionUtil.CanAccess(this.ViewState, email.CommandName);

                            email.Attributes.Add("onclick", "javascript:return openNewWindow('VesselAccounts','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreDocumentCheckListMail.aspx?Crewplanid=" + drv["FLDCREWPLANID"].ToString() + "'); return false;");
                        }
                        if (cmdOfferLetter != null)
                        {
                            cmdOfferLetter.Visible = true;
                            cmdOfferLetter.Attributes.Add("onclick", "parent.openNewWindow('chml', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreOfferLetter.aspx?employeeid="
                            + lblOnSigner.Text
                            + "&crewplanid=" + drv["FLDCREWPLANID"].ToString()
                            + "&appointmentletterid=" + drv["FLDAPPOINTMENTLETTERID"].ToString()
                            + "&popup=1" + "');return false;");
                        }
                        if (cmdApproveTravel != null)
                        {
                            cmdApproveTravel.Visible = true;
                            cmdApproveTravel.Attributes.Add("onclick", "parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreSuitabilityCheckWithDocument.aspx?empid=" + lblOnSigner.Text
                                + "&vesselid=" + VesselId.Text
                                + "&rankid=" + rank.Text
                                + "&crewplanid=" + drv["FLDCREWPLANID"].ToString()                              
                                + "&offsignerid=" + lblOnSigner.Text
                                + "&reliefdate=" + joindate.Text
                                + "&personalmaster=" + personalmaster
                                + "&newapplicant=" + newapplicant
                                + "&popup=1');return false;");
                        }

                        if (cmdIniTravel != null) cmdIniTravel.Visible = true;
                        if (cmdApproveSignOn != null) cmdApproveSignOn.Visible = false;
                        if (cmdAppLetter != null) cmdAppLetter.Visible = false;
                        if (cmdCancelAppointment != null) cmdCancelAppointment.Visible = false;
                    }
                    else if (lblPDStatusID != null && lblPDStatusID.Text == PhoenixCommonRegisters.GetHardCode(1, 99, "AFT")) // Approved for Travel
                    {
                        if (ccmdDocChecklist != null)
                        {
                            ccmdDocChecklist.Visible = true;
                            ccmdDocChecklist.Attributes.Add("onclick", "javascript:parent.openNewWindow('CAPP','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreOfficeDocumentChecklist.aspx?Crewplanid=" + drv["FLDCREWPLANID"].ToString() + "'); return true;");
                        }
                        if (cmdApproveTravel != null) cmdApproveTravel.Visible = false;
                        if (cmdAppLetter != null) cmdAppLetter.Visible = true;
                        if (cmdCancelAppointment != null) cmdCancelAppointment.Visible = true;
                        if (!string.IsNullOrEmpty(lblOffSigner.Text))
                        {
                            if (cmdIniTravel != null) cmdIniTravel.Visible = true;
                            if (cmdApproveSignOn != null)
                            {
                                cmdApproveSignOn.Visible = true;
                                cmdApproveSignOn.Attributes.Add("onclick", "parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreSuitabilityCheckWithDocument.aspx?empid=" + lblOffSigner.Text
                                + "&vesselid=" + VesselId.Text
                                + "&rankid=" + rank.Text
                                + "&crewplanid=" + drv["FLDCREWPLANID"].ToString()                               
                                + "&offsignerid=" + lblOnSigner.Text
                                + "&reliefdate=" + joindate.Text
                                + "&personalmaster=" + personalmaster
                                + "&newapplicant=" + newapplicant
                                + "&popup=1');return false;");
                            }
                        }
                    }
                    else if (lblPDStatusID != null && lblPDStatusID.Text == PhoenixCommonRegisters.GetHardCode(1, 99, "TLG")) // Travelling
                    {
                        if (cmdApproveTravel != null) cmdApproveTravel.Visible = false;
                        if (cmdAppLetter != null) cmdAppLetter.Visible = true;
                        if (cmdCancelAppointment != null) cmdCancelAppointment.Visible = false;
                        if (!string.IsNullOrEmpty(lblOffSigner.Text))
                        {
                            if (cmdIniTravel != null) cmdIniTravel.Visible = true;
                            if (cmdApproveSignOn != null)
                            {
                                cmdApproveSignOn.Visible = true;
                                cmdApproveSignOn.Attributes.Add("onclick", "parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreSuitabilityCheck.aspx?empid=" + lblOffSigner.Text
                                + "&vesselid=" + VesselId.Text
                                + "&rankid=" + rank.Text
                                + "&crewplanid=" + drv["FLDCREWPLANID"].ToString()
                                + "&trainingmatrixid=" + drv["FLDTRAININGMATRIXID"].ToString()
                                + "&offsignerid=" + lblOnSigner.Text
                                + "&reliefdate=" + joindate.Text
                                + "&personalmaster=" + personalmaster
                                + "&newapplicant=" + newapplicant
                                + "&popup=1');return false;");
                            }
                        }
                    }
                    else if (lblPDStatusID != null && lblPDStatusID.Text == PhoenixCommonRegisters.GetHardCode(1, 99, "AFS")) // Approved for sign on                        
                    {
                        if (cmdApproveTravel != null) cmdApproveTravel.Visible = false;
                        if (cmdApproveSignOn != null) cmdApproveSignOn.Visible = false;
                        if (cmdIniTravel != null) cmdIniTravel.Visible = false;
                        if (cmdAppLetter != null) cmdAppLetter.Visible = true;
                        if (cmdCancelAppointment != null) cmdCancelAppointment.Visible = true;
                        if (eb != null) eb.Visible = false;
                        if (cmdRemark != null) cmdRemark.Visible = false;
                        if (db != null) db.Visible = false;
                        if (md != null) md.Visible = false;
                        if (co != null) co.Visible = false;
                        if (sho != null) sho.Visible = false;
                        if (cmdCancelAppointment != null)
                        {
                            cmdCancelAppointment.Attributes.Add("onclick", "javascript:parent.openNewWindow('CAPP','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreCancelAppointmentReasonUpdate.aspx?CREWPLANID=" + drv["FLDCREWPLANID"].ToString()
                                + "&APPOINTMENTLETTERID=" + drv["FLDAPPOINTMENTLETTERID"].ToString()
                                + "&EMPLOYEEID=" + lblOffSigner.Text
                                + "&ISCREWYN=1" + "','medium'); return true;");
                        }
                    }
                }

                RadLabel lblSTCWFlag = (RadLabel)e.Item.FindControl("lblSTCWFlag");
                RadLabel lblCharterer = (RadLabel)e.Item.FindControl("lblCharterer");
                RadLabel lblCompany = (RadLabel)e.Item.FindControl("lblCompany");
                RadLabel lblTrainingmatrixid = (RadLabel)e.Item.FindControl("lblTrainingmatrixid");
                Image imgFlagP = (Image)e.Item.FindControl("ImgFlagP");
                Image imgFlagT = (Image)e.Item.FindControl("ImgFlagT");
                Image imgFlagS = (Image)e.Item.FindControl("ImgFlagS");
                Image imgFlagA = (Image)e.Item.FindControl("ImgFlagA");
                if (lblOffSigner != null && !string.IsNullOrEmpty(lblOffSigner.Text))
                {
                    if (imgFlagP != null) imgFlagP.Attributes.Add("onclick", "parent.openNewWindow('course', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreComplianceCheckList.aspx?employeeid=" + lblOffSigner.Text + "&trainingmatrixid=" + lblTrainingmatrixid.Text + "&stage=1&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "');return false;");
                    if (imgFlagT != null) imgFlagT.Attributes.Add("onclick", "parent.openNewWindow('course', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreComplianceCheckList.aspx?employeeid=" + lblOffSigner.Text + "&trainingmatrixid=" + lblTrainingmatrixid.Text + "&stage=2&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "');return false;");
                    if (imgFlagS != null) imgFlagS.Attributes.Add("onclick", "parent.openNewWindow('course', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreComplianceCheckList.aspx?employeeid=" + lblOffSigner.Text + "&trainingmatrixid=" + lblTrainingmatrixid.Text + "&stage=3&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "');return false;");
                    if (imgFlagA != null) imgFlagA.Attributes.Add("onclick", "parent.openNewWindow('course', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreComplianceCheckList.aspx?employeeid=" + lblOffSigner.Text + "&trainingmatrixid=" + lblTrainingmatrixid.Text + "&stage=4&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "');return false;");
                }

                
              
            }
            else
            {
                e.Item.Attributes["onclick"] = "";
            }

            RadLabel lblRemarks = (RadLabel)e.Item.FindControl("lblRemarks");
            LinkButton imgRemarks = (LinkButton)e.Item.FindControl("imgRemarks");

          
            if (cmdRemark != null && lblCrewPlanId != null && lblOnSigner != null)
                cmdRemark.Attributes.Add("onclick", "openNewWindow('Attachment','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreReliefPlanRemarks.aspx?crewplanid=" + lblCrewPlanId.Text + "&empid=" + lblOnSigner.Text + "',false,400,300);");


            //UserControlSeaport ucSeaPort = (UserControlSeaport)e.Item.FindControl("ddlPlannedPort");
            //DataRowView drvSeaPort = (DataRowView)e.Item.DataItem;
            //if (ucSeaPort != null)
            //    ucSeaPort.SelectedSeaport = drvSeaPort["FLDSEAPORTID"].ToString();

            LinkButton lnkRelievee = (LinkButton)e.Item.FindControl("lnkRelievee");
            RadLabel lblName = (RadLabel)e.Item.FindControl("lblName");
            LinkButton lnkReliever = (LinkButton)e.Item.FindControl("lnkReliever");
            RadLabel lblRelieverName = (RadLabel)e.Item.FindControl("lblRelieverName");

            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName)) eb.Visible = false;
            }
            if (cmdRemark != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdRemark.CommandName)) cmdRemark.Visible = false;
            }
            if (pd != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, pd.CommandName)) pd.Visible = false;
            }
            if (sho != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, sho.CommandName)) sho.Visible = false;
            }
            if (md != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, md.CommandName)) md.Visible = false;
            }
            if (co != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, co.CommandName)) co.Visible = false;
            }
            if (cmdIniTravel != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdIniTravel.CommandName)) cmdIniTravel.Visible = false;
            }
            if (cmdApproveTravel != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdApproveTravel.CommandName)) cmdApproveTravel.Visible = false;
            }
            if (cmdApproveSignOn != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdApproveSignOn.CommandName)) cmdApproveSignOn.Visible = false;
            }
            if (cmdAppLetter != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdAppLetter.CommandName)) cmdAppLetter.Visible = false;
            }
            if (cmdCancelAppointment != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdCancelAppointment.CommandName)) cmdCancelAppointment.Visible = false;
            }
            if (cmdAppointmentLetter != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdAppointmentLetter.CommandName)) cmdAppointmentLetter.Visible = false;
            }

            if(lblOnSigner.Text == "-1")
            {
                if (db != null)
                {
                    db.Visible = false;
                }

                if (eb != null)
                {
                    eb.Visible = false;
                }
                if (cmdRemark != null)
                {
                    cmdRemark.Visible = false;
                }
                if (pd != null)
                {
                   pd.Visible = false;
                }
                if (sho != null)
                {
                   sho.Visible = false;
                }
                if (md != null)
                {
                  md.Visible = false;
                }
                if (co != null)
                {
                   co.Visible = false;
                }
                if (cmdIniTravel != null)
                {
                    cmdIniTravel.Visible = false;
                }
                if (cmdApproveTravel != null)
                {
                   cmdApproveTravel.Visible = false;
                }
                if (cmdApproveSignOn != null)
                {
                   cmdApproveSignOn.Visible = false;
                }
                if (cmdAppLetter != null)
                {
                   cmdAppLetter.Visible = false;
                }
                if (cmdCancelAppointment != null)
                {
                   cmdCancelAppointment.Visible = false;
                }
                if (cmdAppointmentLetter != null)
                {
                   cmdAppointmentLetter.Visible = false;
                }
            }
        }

    }



    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        gvEvent.Rebind();
    }




    protected void lnkSaveRemarks_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixCrewChangeEvent.UpdateCrewPlanEvent(new Guid(ViewState["eventid"].ToString()), txtRemarks.Text);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


}