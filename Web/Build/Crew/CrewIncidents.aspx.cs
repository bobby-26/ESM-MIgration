using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewIncidents : PhoenixBasePage
{    
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridDataItem r in gvIncidents.Items)
    //    {
          
    //            Page.ClientScript.RegisterForEventValidation
    //                    (r.UniqueID + "$ctl00");
    //            Page.ClientScript.RegisterForEventValidation
    //                    (r.UniqueID + "$ctl01");
           
    //    }
    //    base.Render(writer);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "display:none;");
        txtSeaPortId.Attributes.Add("style", "visibility:hidden;");
        txtSeaportCode.Attributes.Add("style", "visibility:hidden;");
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        try
        {
           
            if (!IsPostBack)
            {
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                toolbar.AddButton("New", "NEW", ToolBarDirection.Right);

                MenuIncidents.AccessRights = this.ViewState;
                MenuIncidents.MenuList = toolbar.Show();
                DataSet ds = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 11);
                DataRow[] dr = ds.Tables[0].Select("FLDSHORTNAME = '" + (Request.QueryString["t"] == "c" ? "COM" : "COP") + "'"); // short code of quick register
                ViewState["CORRESTYPE"] = dr.Length > 0 ? dr[0]["FLDQUICKCODE"].ToString() : string.Empty;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["INCIDENTDTKEY"] = string.Empty;

                gvIncidents.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Crew/CrewIncidents.aspx?empid=" + Request.QueryString["empid"], "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvIncidents')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        if (ViewState["INCIDENTDTKEY"].ToString() != string.Empty)
        {
            SetIncidentDetail(new Guid(ViewState["INCIDENTDTKEY"].ToString()));
        }
    }
    protected void CrewShowExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

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
    protected void ShowExcel()
    {
        string[] alColumns = { "FLDVESSELNAME", "FLDINCIDENTDATE", "FLDREPORTEDBY", "FLDCORRESPONDENCETYPE" };
        string[] alCaptions = { "Vessel", "Incident Date", "Reported By", "Compliments/Complaints" };

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression;
        int? sortdirection = 1;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt;
        dt = PhoenixCrewIncidents.SearchIncidents(null, null, General.GetNullableInteger(ViewState["CORRESTYPE"].ToString()) 
                                            , sortexpression, sortdirection
                                            , 1
                                            , iRowCount
                                            , ref iRowCount
                                            , ref iTotalPageCount);
        General.ShowExcel("Compliments/Complaints", dt, alColumns, alCaptions, sortdirection, sortexpression);
       
    }
    protected void Incidents_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {

                if (!IsValidIncident())
                {
                    ucError.Visible = true;
                    return;
                }
                if (string.IsNullOrEmpty(ViewState["INCIDENTDTKEY"].ToString()))
                {
                    PhoenixCrewIncidents.InsertIncidents(int.Parse(ddlIncidentType.SelectedHard), int.Parse(ddlVessel.SelectedVessel), General.GetNullableDateTime(txtIncidentDate.Text).Value
                                                            , txtReportedBy.Text, General.GetNullableInteger(txtSeaPortId.Text)
                                                            , txtVoyageNumber.Text, General.GetNullableDateTime(txtReportedDate.Text)
                                                            , General.GetNullableInteger(ddlRank.SelectedRank), txtRemarks.Content, General.GetNullableInteger(ViewState["CORRESTYPE"].ToString()), ReadCheckBoxList(lstCrewList));
                    ResetFields();
                }
                else
                {
                    PhoenixCrewIncidents.UpdateIncidents(new Guid(ViewState["INCIDENTDTKEY"].ToString()), int.Parse(ddlVessel.SelectedVessel), General.GetNullableDateTime(txtIncidentDate.Text).Value
                                                           , txtReportedBy.Text, General.GetNullableInteger(txtSeaPortId.Text)
                                                           , txtVoyageNumber.Text, General.GetNullableDateTime(txtReportedDate.Text)
                                                           , General.GetNullableInteger(ddlRank.SelectedRank), txtRemarks.Content, ReadCheckBoxList(lstCrewList));
                    ucStatus.Text = "Information Updated";
                    SetIncidentDetail(new Guid(ViewState["INCIDENTDTKEY"].ToString()));
                }
                gvIncidents.Rebind();

            }
            else if (CommandName.ToUpper().Equals("NEW"))
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                toolbar.AddButton("New", "NEW", ToolBarDirection.Right);

                MenuIncidents.AccessRights = this.ViewState;
                MenuIncidents.MenuList = toolbar.Show();
                ResetFields();

                imgClip.Visible = false;
                gvIncidents.Rebind();
            }
            else if (CommandName.ToUpper().Equals("EVALUATIONSUGGESTION"))
            {

                String script = String.Format("javascript:openNewWindow('CI','','" + Session["sitepath"] + "/Crew/CrewIncidentsEvaluationSuggestion.aspx?id=" + ViewState["FLDDTKEY"].ToString() + "');");

                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

            }
            else if (CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                String script = String.Format("javascript:openNewWindow('CI','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["FLDDTKEY"].ToString() + "&mod=" + PhoenixModule.CREW + "');");

                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
            else if (CommandName.ToUpper().Equals("LOCK") || CommandName.ToUpper().Equals("UNLOCK"))
            {
                String script = String.Format("javascript:parent.openNewWindow('CI','','" + Session["sitepath"] + "/Crew/CrewIncidentsLockAndUnlock.aspx?id=" + ViewState["FLDDTKEY"].ToString() + "','medium');");

                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetIncidentDetail(Guid iIncidentDtkey)
    {
        DataTable dt = PhoenixCrewIncidents.ListIncidents(iIncidentDtkey, null);

        if (dt.Rows.Count > 0)
        {
           
           
           

            ViewState["INCIDENTDTKEY"] = dt.Rows[0]["FLDDTKEY"].ToString();
            ddlVessel.SelectedVessel = dt.Rows[0]["FLDVESSELID"].ToString();            
            txtSeaPortId.Text = dt.Rows[0]["FLDSEAPORTID"].ToString();
            txtSeaPortName.Text = dt.Rows[0]["FLDSEAPORTNAME"].ToString();
            txtVoyageNumber.Text = dt.Rows[0]["FLDVOYAGENO"].ToString();
            txtReportedDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDREPORTEDDATE"].ToString());
            txtIncidentDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDINCIDENTDATE"].ToString());
            txtReportedBy.Text = dt.Rows[0]["FLDREPORTEDBY"].ToString();
            ddlIncidentType.SelectedHard = dt.Rows[0]["FLDINCIDENTTYPE"].ToString();
            ddlRank.SelectedRank = dt.Rows[0]["FLDRANKID"].ToString();
            txtRemarks.Content = dt.Rows[0]["FLDREMARKS"].ToString();
            ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
            BindSeafarer(null, null);
            BindCheckBoxList(lstCrewList, dt.Rows[0]["FLDCREWLIST"].ToString());
            SetAttachmentMarking();
            PhoenixToolbar toolbar = new PhoenixToolbar();
           
            if (dt.Rows[0]["FLDSTATUS"].ToString() == string.Empty)
            {
                toolbar.AddButton("Lock", "LOCK", ToolBarDirection.Right);
                toolbar.AddButton("Attachment", "ATTACHMENT", ToolBarDirection.Right);
                toolbar.AddButton("Evaluation & Suggestion", "EVALUATIONSUGGESTION", ToolBarDirection.Right);
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
               
                
                
                ViewState["FLDDTKEY"] = dt.Rows[0]["FLDDTKEY"].ToString();
                //toolbar.AddImageLink("javascript:parent.Openpopup('CI','','../Crew/CrewIncidentsEvaluationSuggestion.aspx?id=" + dt.Rows[0]["FLDDTKEY"].ToString() + "'); return false;", "Evaluation & Suggestion", "", "EVALUATIONSUGGESTION");
                //toolbar.AddImageLink("javascript:parent.Openpopup('CI','','../Common/CommonFileAttachment.aspx?dtkey=" + dt.Rows[0]["FLDDTKEY"].ToString() + "&mod="
                //      + PhoenixModule.CREW + "'); return false;", "Attachment", "", "ATTACHMENT");
                //toolbar.AddImageLink("javascript:parent.Openpopup('CI','','../Crew/CrewIncidentsLockAndUnlock.aspx?id=" + dt.Rows[0]["FLDDTKEY"].ToString() + "','medium'); return false;", "Lock", "", "LOCK");
            }
            else
            {
                toolbar.AddButton("UnLock", "UNLOCK", ToolBarDirection.Right);
                toolbar.AddButton("Attachment", "ATTACHMENT", ToolBarDirection.Right);
                toolbar.AddButton("Evaluation & Suggestion", "EVALUATIONSUGGESTION", ToolBarDirection.Right);
                
                
                //toolbar.AddImageLink("javascript:parent.Openpopup('CI','','../Crew/CrewIncidentsEvaluationSuggestion.aspx?u=n&id=" + dt.Rows[0]["FLDDTKEY"].ToString() + "'); return false;", "Evaluation & Suggestion", "", "EVALUATIONSUGGESTION");
                //toolbar.AddImageLink("javascript:parent.Openpopup('CI','','../Common/CommonFileAttachment.aspx?u=n&dtkey=" + dt.Rows[0]["FLDDTKEY"].ToString() + "&mod="
                //      + PhoenixModule.CREW + "'); return false;", "Attachment", "", "ATTACHMENT");
                //toolbar.AddImageLink("javascript:parent.Openpopup('CI','','../Crew/CrewIncidentsLockAndUnlock.aspx?id=" + dt.Rows[0]["FLDDTKEY"].ToString() + "','medium'); return false;", "UnLock", "", "UNLOCK");
            }
            toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
            MenuIncidents.AccessRights = this.ViewState;
            MenuIncidents.MenuList = toolbar.Show();

        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
      
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = 1;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string[] alColumns = { "FLDVESSELNAME", "FLDINCIDENTDATE", "FLDREPORTEDBY", "FLDCORRESPONDENCETYPE" };
        string[] alCaptions = { "Vessel", "Incident Date", "Reported By", "Compliments/Complaints" };


        DataTable dt = PhoenixCrewIncidents.SearchIncidents(null, null, General.GetNullableInteger(ViewState["CORRESTYPE"].ToString())
                                            , sortexpression, sortdirection
                                            , (int)ViewState["PAGENUMBER"]
                                            , gvIncidents.PageSize
                                            , ref iRowCount
                                            , ref iTotalPageCount);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvIncidents", "Compliments/Complaints", alCaptions, alColumns, ds);
        gvIncidents.DataSource = dt;
        gvIncidents.VirtualItemCount = iRowCount;

      
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
   
   
   
    private void ResetFields()
    {
        ViewState["INCIDENTDTKEY"] = string.Empty;
        ddlVessel.SelectedVessel = string.Empty;
        txtSeaportCode.Text = "";
        txtSeaPortName.Text = "";
        txtSeaPortId.Text = "";
        txtVoyageNumber.Text = string.Empty;
        txtReportedDate.Text = string.Empty;
        txtIncidentDate.Text = string.Empty;
        txtReportedBy.Text = string.Empty;
        ddlIncidentType.SelectedHard = string.Empty;
        ddlRank.SelectedRank = string.Empty;
        txtRemarks.Content = string.Empty;
        lstCrewList.Items.Clear();
    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
    }  

    public bool IsValidIncident()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        int resultInt;
        if (string.IsNullOrEmpty(txtIncidentDate.Text) || !DateTime.TryParse(txtIncidentDate.Text, out resultDate))
            ucError.ErrorMessage = "Incident date is required.";
        else if (DateTime.TryParse(txtIncidentDate.Text, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Incident date should be earlier than current date";
        }
        
        if (!string.IsNullOrEmpty(txtReportedDate.Text) &&
                DateTime.TryParse(txtReportedDate.Text, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Reported date should be earlier than current date";
        }
        else if (!string.IsNullOrEmpty(txtReportedDate.Text) && DateTime.TryParse(txtIncidentDate.Text, out resultDate)
                && DateTime.TryParse(txtReportedDate.Text, out resultDate) && DateTime.Compare(DateTime.Parse(txtIncidentDate.Text), resultDate) > 0)
            ucError.ErrorMessage = "Reported date should be later than Incident date";            

        if (string.IsNullOrEmpty(ddlVessel.SelectedVessel) || !int.TryParse(ddlVessel.SelectedVessel,out resultInt))
            ucError.ErrorMessage = "Vessel is required.";

        if (string.IsNullOrEmpty(txtReportedBy.Text))
            ucError.ErrorMessage = "Reported By is required.";

        if (string.IsNullOrEmpty(ddlIncidentType.SelectedHard) || !int.TryParse(ddlIncidentType.SelectedHard, out resultInt))
            ucError.ErrorMessage = "Type of Incident is required.";
        if(ReadCheckBoxList(lstCrewList).Equals(""))
            ucError.ErrorMessage = "Crew List is required.";

        return (!ucError.IsError);

    }
    

    private void SetAttachmentMarking()
    {
        DataTable dt1 = PhoenixCommonFileAttachment.AttachmentList(new Guid(ViewState["attachmentcode"].ToString()));
        if (dt1.Rows.Count > 0)
        {
            imgClip.Visible = true;
            imgClip.Attributes["onclick"] = "javascript:parent.Openpopup('NAA','','../Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
                   + PhoenixModule.CREW + "'); return false;";
        }
        else
            imgClip.Visible = false;
    }
    protected void cmdClear_Click(object sender, ImageClickEventArgs e)
    {
        txtSeaportCode.Text = "";
        txtSeaPortName.Text = "";
        txtSeaPortId.Text = "";
    }

    protected void BindSeafarer(object sender, EventArgs e)
    {
        if (General.GetNullableInteger(ddlVessel.SelectedVessel).HasValue && General.GetNullableDateTime(txtIncidentDate.Text).HasValue)
        {
            DataSet ds = PhoenixCrewManagement.ListCrewOnboard(General.GetNullableInteger(ddlVessel.SelectedVessel), null
               , General.GetNullableDateTime(txtIncidentDate.Text), null);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                lstCrewList.Items.Clear();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    lstCrewList.Items.Add(new ButtonListItem(dr["FLDRANKCODE"].ToString() + " - " + dr["FLDEMPLOYEENAME"].ToString(), dr["FLDEMPLOYEEID"].ToString()));
                }
            }
            else
            {
                lstCrewList.Items.Clear();
            }
        }
        else
        {
            lstCrewList.Items.Clear();
        }
    }
    private void BindCheckBoxList(RadCheckBoxList cbl, string list)
    {
        foreach (string item in list.Split(','))
        {
            if (item.Trim() != "")
            {
                cbl.SelectedValue = item;
            }
        }
    }
    private string ReadCheckBoxList(RadCheckBoxList cbl)
    {
        string list = string.Empty;

        foreach (ButtonListItem item in cbl.Items)
        {
            if (item.Selected == true)
            {
                list = list + item.Value.ToString() + ",";
            }
        }
        list = list.TrimEnd(',');
        return list;
    }


    protected void gvIncidents_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvIncidents.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvIncidents_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
           
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                RadLabel lblId = (RadLabel)e.Item.FindControl("lblIncidentId");
                LinkButton cl = (LinkButton)e.Item.FindControl("cmdCrewList");
                cl.Attributes.Add("onclick", "parent.openNewWindow('CI','','"+Session["sitepath"]+"/Crew/CrewIncidentsEmployeeList.aspx?id=" + lblId.Text + "'); return false;");
           
        }
       
        
    }

    protected void gvIncidents_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
        if(e.CommandName.ToUpper() == "DELETE")
        {
            try
            {
              
                RadLabel lblIncidentId = (RadLabel)e.Item.FindControl("lblIncidentId");
                PhoenixCrewIncidents.DeleteIncidents(new Guid(lblIncidentId.Text));
                gvIncidents.Rebind();

             
                ResetFields();
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;

            }
        }
        if(e.CommandName.ToUpper()=="EDIT")
        {
            try
            {
              
                RadLabel lblIncidentId = (RadLabel)e.Item.FindControl("lblIncidentId");
                SetIncidentDetail(new Guid(lblIncidentId.Text));
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }

    }

    protected void gvIncidents_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
}
