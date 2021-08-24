using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;
using SouthNests.Phoenix.VesselAccounts;
using System.Text;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Collections;
using System.Web;

public partial class PlannedMaintenanceWorkOrderGroupToolBoxMeet : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Refresh", "REFRESH", ToolBarDirection.Right);
        toolbar.AddButton("Submit", "SAVE", ToolBarDirection.Right);
        MenuToolBoxMeet.AccessRights = this.ViewState;
        MenuToolBoxMeet.MenuList = toolbar.Show();

        PhoenixToolbar toolbarprint = new PhoenixToolbar();
        toolbarprint.AddImageButton("", "Print Toolbox", "pdf.png", "TOOLBOX");
        Menutoolboxprint.AccessRights = this.ViewState;
        Menutoolboxprint.MenuList = toolbarprint.Show();
        ScriptManager.GetCurrent(this).RegisterPostBackControl(Menutoolboxprint);
        RadScriptManager.GetCurrent(this).RegisterPostBackControl(gvToolBoxList);


        if (!IsPostBack)
        {
            ViewState["PROCEEDYN"] = 1;
            ViewState["PROCEEDERROR"] = string.Empty;

            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            gvWaiver.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            ViewState["VESSELID"] = "0";
            if (Request.QueryString["vesselid"] != null)
                ViewState["VESSELID"] = Request.QueryString["vesselid"];
            else
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

            ViewState["GroupId"] = string.Empty;
            if (Request.QueryString["groupId"] != null)
            {
                ViewState["GroupId"] = Request.QueryString["groupId"].ToString();
            }
            txtToolBoxMeet.SelectedDate = DateTime.Now.Date;
            txtToolBoxMeetTime.SelectedTime = DateTime.Now.TimeOfDay;

            if (Request.QueryString["WONUMBER"] != null)
            {
                txtWorkorderNumber.Text = Request.QueryString["WONUMBER"].ToString();
            }
            ViewState["DWPID"] = Request.QueryString["wopid"] ?? string.Empty;
            BindCrewList();
            EditDailyWorkPlan();
        }
    }
    
    private bool IsValidData()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(ddlPersonIncharge.SelectedValue))
            ucError.ErrorMessage = "PIC is required";
        //var collection = ddlOtherMembers.CheckedItems;
        //if (collection.Count == 0)
        //{
        //    ucError.ErrorMessage = " Others is required";
        //}
        return (!ucError.IsError);
    }

    private void BindCrewList()
    {
        DataTable dt = PhoenixVesselAccountsEmployee.ListVesselCrew(int.Parse(ViewState["VESSELID"].ToString()), General.GetNullableDateTime(""));

        ddlPersonIncharge.DataSource = dt;
        ddlPersonIncharge.DataBind();

        ddlOtherMembers.DataSource = dt;
        ddlOtherMembers.DataBind();

    }

    private void EditDailyWorkPlan()
    {
        Guid? id = General.GetNullableGuid(ViewState["DWPID"].ToString());
        if (id.HasValue)
        {
            DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlan.EditWorkOrder(id.Value, int.Parse(ViewState["VESSELID"].ToString()));
            ddlPersonIncharge.SelectedValue = dt.Rows[0]["FLDPERSONINCHARGEID"].ToString();
            foreach (RadComboBoxItem item in ddlOtherMembers.Items)
            {
                item.Checked = false;
                if (dt.Rows[0]["FLDOTHERMEMBERS"].ToString().Contains("," + item.Value + ","))
                {
                    item.Checked = true;
                }
            }
        }
    }
    protected void gvToolBoxList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt;
        if (Request.QueryString["vesselid"] != null)
            dt = PhoenixPlannedMaintenanceWorkOrderGroup.WorkorderGroupToolBoxList(new Guid(ViewState["GroupId"].ToString()), int.Parse(ViewState["VESSELID"].ToString()));
        else
            dt = PhoenixPlannedMaintenanceWorkOrderGroup.WorkorderGroupToolBoxList(new Guid(ViewState["GroupId"].ToString()));
        gvToolBoxList.DataSource = dt;
    }

    protected void MenuToolBoxMeet_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!string.IsNullOrEmpty(ViewState["GroupId"].ToString()))
                {
                    string reportDatetime = txtToolBoxMeet.SelectedDate.Value.ToShortDateString() + " " + txtToolBoxMeetTime.SelectedTime;
                    string picid = ddlPersonIncharge.SelectedValue;
                    var collection = ddlOtherMembers.CheckedItems;
                    if (!IsValidData())
                    {
                        ucError.Visible = true;
                        return;
                    }
                    if (ViewState["PROCEEDYN"].ToString() == "0")
                    {
                        ucError.ErrorMessage = ViewState["PROCEEDERROR"].ToString();
                        ucError.Visible = true;
                        return;
                    }

                    string picname = ddlPersonIncharge.Text;

                    string csvOtherMembers = string.Empty;
                    string csvOtherMembersName = string.Empty;
                    if (collection.Count != 0)
                    {
                        csvOtherMembers = ",";
                        csvOtherMembersName = ",";
                        foreach (var item in collection)
                        {
                            csvOtherMembers = csvOtherMembers + item.Value + ",";
                            csvOtherMembersName = csvOtherMembersName + item.Text + ",";
                        }
                    }
                    PhoenixPlannedMaintenanceWorkOrderGroup.WorkorderGroupToolBoxInsert(new Guid(ViewState["GroupId"].ToString())
                                                                                     , DateTime.Parse(reportDatetime)
                                                                                     , General.GetNullableInteger(picid)
                                                                                     , picname, csvOtherMembers, csvOtherMembersName
                                                                                     , null, null, null
                                                                                     , int.Parse(ViewState["VESSELID"].ToString()));

                    //PhoenixPlannedMaintenanceWorkOrderGroup.PlannedJobToolboxCheck(new Guid(ViewState["GroupId"].ToString()));

                    gvToolBoxList.Rebind();
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
            "BookMarkScript", "refreshParent(false);", true);
            }
            else if (CommandName.ToUpper().Equals("REFRESH"))
            {
                PhoenixPlannedMaintenanceWorkOrderGroup.RefreshToolboxMeet(new Guid(ViewState["GroupId"].ToString()));
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
            "BookMarkScript", "refreshParent(true);", true);
            }           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvToolBoxList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("PRINT"))
        {

            string toolboxId = (e.Item as GridDataItem).GetDataKeyValue("FLDTOOLBOXMEETID").ToString();
            ConvertToPdf(PrepareHtmlDoc(toolboxId));
        }

    }    

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
    }

    protected void gvWaiver_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindRequiredItemStatus();
    }

    protected void gvWaiver_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        LinkButton compNo = (LinkButton)e.Item.FindControl("lblComponentNo");
        if(compNo != null)
        {
            compNo.Visible = SessionUtil.CanAccess(this.ViewState, compNo.CommandName);
            compNo.Attributes.Add("onclick", "javascript:openNewWindow('PTW','','" + Session["sitepath"] + "/Inventory/InventoryComponentGeneral.aspx?COMPONENTID="+ drv["FLDCOMPONENTID"]+"&p=0',500,600); return false;");
        }
        LinkButton job = (LinkButton)e.Item.FindControl("lblWorkordername");
        if (job != null)
        {
            job.Visible = SessionUtil.CanAccess(this.ViewState, job.CommandName);
            if (General.GetNullableGuid(drv["FLDCOMPONENTJOBID"].ToString()) != null)
            {
                if (Request.QueryString["vesselid"] != null)
                    job.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJob.aspx?COMPONENTJOBID=" + drv["FLDCOMPONENTJOBID"] + "&COMPONENTID=" + drv["FLDCOMPONENTID"] + "&Cancelledjob=0&vesselid=" + int.Parse(ViewState["VESSELID"].ToString()) + "'); ");
                else
                    job.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJob.aspx?COMPONENTJOBID=" + drv["FLDCOMPONENTJOBID"] + "&COMPONENTID=" + drv["FLDCOMPONENTID"] + "&Cancelledjob=0'); ");
            }
            else
            {
                if (Request.QueryString["vesselid"] != null)
                    job.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderDetail.aspx?WORKORDERID=" + drv["FLDWORKORDERID"].ToString() + "&vesselid=" + int.Parse(ViewState["VESSELID"].ToString()) + "'); ");
                else
                    job.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderDetail.aspx?WORKORDERID=" + drv["FLDWORKORDERID"].ToString() + "'); ");
            }
        }
    }

    private void BindRequiredItemStatus()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDPLANNINGDUEDATE", "FLDWORKORDERSTATUS" };
            string[] alCaptions = { "Comp. No", "Comp. Name", "Job No", "Job Title", "Due Date", "Status" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixPlannedMaintenanceWorkOrderGroup.ToolboxValidationChecks(new Guid(ViewState["GroupId"].ToString()),
                sortexpression, sortdirection, gvWaiver.CurrentPageIndex + 1, gvWaiver.PageSize, ref iRowCount, ref iTotalPageCount);

            gvWaiver.DataSource = ds;

            gvWaiver.VirtualItemCount = iRowCount;
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWaiver_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }



    protected void Menutoolboxprint_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("TOOLBOX"))
            {
                ConvertToPdf(PrepareHtmlDoc(null));
            }           
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private string PrepareHtmlDoc(string toolboxid)
    {
        DataSet dsPDF = PhoenixPlannedMaintenanceWorkOrderGroup.SubWorkOrderToolboxMeetingForm(new Guid(ViewState["GroupId"].ToString()),General.GetNullableGuid(toolboxid));

        StringBuilder DsHtmlcontent = new StringBuilder();

        if (dsPDF.Tables[0].Rows.Count > 0)
        {
            DataTable main = dsPDF.Tables[0];
            DataRow maindr = dsPDF.Tables[1].Rows[0];
            DsHtmlcontent.Append("<html>");
            DsHtmlcontent.Append("<table style=\"width:100%; font-size: 10px;\"><tr><td>" + PhoenixSecurityContext.CurrentSecurityContext.VesselName +"</td>");
            DsHtmlcontent.Append("<td>Work Order</td></tr></table>");
            DsHtmlcontent.Append("<table style=\"width:100%;font-size: 7px;\" border=\"1\">");
            DsHtmlcontent.Append("<tr style=\"background-color:#96bae6;font:bold;text-align:center;\">");
             DsHtmlcontent.Append("<th>Work Order No: " + maindr["FLDWORKORDERNUMBER"].ToString() + "</th>");
            DsHtmlcontent.Append("<th colspan=\"4\">Work Order Name: " + maindr["FLDWORKORDERNAME"].ToString() + "</th>");
            DsHtmlcontent.Append("<th colspan=\"2\">Category: " + maindr["FLDJOBCATEGORY"].ToString() + "</th>");
            DsHtmlcontent.Append("<th>Plan Date: " + General.GetDateTimeToString(maindr["FLDPLANNINGDUEDATE"].ToString()) + "</th>");
            DsHtmlcontent.Append("<th>Duration: " + maindr["FLDPLANNINGESTIMETDURATION"].ToString() + "</th>");
            DsHtmlcontent.Append("<th colspan=\"3\">Responsibility: " + maindr["FLDDISCIPLINENAME"].ToString() + "</th>");
            DsHtmlcontent.Append("<th>Status: " + maindr["FLDSTATUS"].ToString() + "</th></tr>");
            //DsHtmlcontent.Append("<table style=\"width:100%; font-size: 7px;\" border=\"1\">");
            //DsHtmlcontent.Append("<tr><th>Comp. No. & Name </th><th>Job Code & Title</th><th>Detail of Checks</th><th>Checked</th><th>Risk Assessment</th><th>JHA</th><th>PTW</th><th>Parameters/Template</th><th>Parts Required</th><th>Remarks</th></tr>");
            DsHtmlcontent.Append("<tr style=\"font:bold;text-align:center;\">");
            DsHtmlcontent.Append("<td>Comp. No. & Name</td><td>Job Code & Title</td><td colspan=\"2\">Detail of Checks</td><td>Checked</td><td>Risk Assessment</td><td>JHA</td><td>PTW</td><td>Parameters/Template</td><td>Parts Required</td><td>Attachment Req</td><td>Details</td><td>Remarks</td></tr>");

            for (int i = 0; i < dsPDF.Tables[0].Rows.Count; i++)
            {
                DataRow dr = dsPDF.Tables[0].Rows[i];

                string RANumber = string.Empty;
                
                if (string.IsNullOrEmpty(dr["FLDRAID"].ToString()) && dr["FLDRAREQUIRED"].ToString() == "1")
                {
                    RANumber = "Required";
                }
                else if (!string.IsNullOrEmpty(dr["FLDRAID"].ToString()))
                {
                    RANumber = dr["FLDRANUMBER"].ToString();
                }
                else
                {
                    RANumber = "NA";
                }

                string partsRequired = string.Empty;
                if (dr["FLDCATEGORYLEVEL"].ToString() == "2")
                {
                    partsRequired = "NA";
                }
                else
                {
                    partsRequired = "Show";
                }
                string template = dr["FLDTEMPLATE"].ToString() == string.Empty ? dr["FLDPARAMETER"].ToString().Trim(',') : dr["FLDTEMPLATE"].ToString().Trim(',');
                DsHtmlcontent.Append("<tr><td>" + dr["FLDCOMPONENTNUMBER"].ToString() + " - " + dr["FLDCOMPONENTNAME"].ToString() + "</td>");
                //DsHtmlcontent.Append("<td>" +  + "</td>");
                DsHtmlcontent.Append("<td>" + dr["FLDWORKORDERNAME"].ToString() + "</td>");
                DsHtmlcontent.Append("<td colspan=\"2\">" + dr["FLDJOBDETAILCHECK"].ToString()+"</td>");
                //DsHtmlcontent.Append("<td>" + dr["FLDJOBCATEGORY"].ToString() + "</td>");
                //DsHtmlcontent.Append("<td>" + dr["FLDFREQUENCYNAME"].ToString() + "</td>");
                //DsHtmlcontent.Append("<td>" + dr["FLDPLANNINGDUEDATE"].ToString() + "</td>");
                DsHtmlcontent.Append("<td>Yes / No / Defect</td>");
                DsHtmlcontent.Append("<td>" + RANumber + "</td>");
                DsHtmlcontent.Append("<td>" + dr["FLDJHA"].ToString().Trim(',') + "</td>");
                DsHtmlcontent.Append("<td>" + dr["FLDPTW"].ToString().Trim(',') + "</td>");
                //DsHtmlcontent.Append("<td>" + "-" + "</td></tr>");
                DsHtmlcontent.Append("<td>" + template + "</td>");
                DsHtmlcontent.Append("<td>" + dr["FLDPARTSREQUIRED"].ToString().Trim(',') + "</td>");
                DsHtmlcontent.Append("<td>" + dr["FLDATTREQUIRED"].ToString()  + "</td>");
                DsHtmlcontent.Append("<td>" + dr["FLDATTINSTRUCTIONS"].ToString() + "</td>");
                DsHtmlcontent.Append("<td></td>");
                DsHtmlcontent.Append("</tr>");
            }

            DsHtmlcontent.Append("</table>");
            DsHtmlcontent.Append("<table style=\"width:100%; font-size:7px;font:Helvetica, Arial, sans-serif \">");
            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td colspan=\"3\">Additional Precautions:</td>");
            DsHtmlcontent.Append("<td colspan=\"3\">Notes:</td>");
            DsHtmlcontent.Append("</tr>");
            DsHtmlcontent.Append("</table>");
            DsHtmlcontent.Append("<table height=\"100px\" border=\"1\" style=\"width:100%;font-size:7px;padding:5px;\">");
            DsHtmlcontent.Append("<tr style=\"height:100px\"><td><br><br></td><td><br><br></td>");
            DsHtmlcontent.Append("</tr>");
            DsHtmlcontent.Append("</table>");
            DsHtmlcontent.Append("<table style=\"width:100%; font-size:7px;\">");
            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td colspan=\"3\">Person in Charge: "+ main.Rows[0]["FLDPIC"].ToString()+"</td>");
            DsHtmlcontent.Append("<td colspan=\"3\">Other Team Members: " +main.Rows[0]["FLDOTHER"].ToString().Trim(',') + "</td>");
            DsHtmlcontent.Append("</tr>");
            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td colspan=\"5\">Signature of PIC: -----------------------------------</td>");
            DsHtmlcontent.Append("<td></td>");
            DsHtmlcontent.Append("</tr>");
            DsHtmlcontent.Append("</table>");
            DsHtmlcontent.Append("</html>");

            //for (int i = 0; i < dsPDF.Tables[0].Rows.Count; i++)
            //{
            //    DataRow dr = dsPDF.Tables[0].Rows[i];
            //    string jobcode = dr["FLDJOBCODE"].ToString();
            //    string jobtitle = dr["FLDJOBTITLE"].ToString();
            //    string description = dr["FLDJOBDETAILS"].ToString();

            //    DsHtmlcontent.Append("<b><u>JOB CODE: " + jobcode + " - " + jobtitle + "</u></b>");
            //    DsHtmlcontent.Append("<p style=\"width:100%; font-size: 10px;\">" + HttpUtility.HtmlDecode(description) + "</p>");
            //}

        }
        return DsHtmlcontent.ToString();

    }
    private void ConvertToPdf(string HTMLString)
    {
        try
        {
            if (HTMLString != "")
            {

                using (var ms = new MemoryStream())
                {
                    iTextSharp.text.Document document = new iTextSharp.text.Document(new iTextSharp.text.Rectangle(595f, 842f));
                    document.SetMargins(36f, 36f, 30f, 0f);
                    document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                    string filefullpath = "ToolboxMeetingForm" + ".pdf";
                    PdfWriter.GetInstance(document, ms);
                    document.Open();

                    StyleSheet styles = new StyleSheet();
                    styles.LoadStyle(".headertable td", "background-color", "Blue");
                    ArrayList htmlarraylist = (ArrayList)iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(HTMLString), styles);

                    for (int k = 0; k < htmlarraylist.Count; k++)
                    {
                        document.Add((iTextSharp.text.IElement)htmlarraylist[k]);
                    }
                    document.Close();
                    HttpContext.Current.Response.Buffer = true;
                    var bytes = ms.ToArray();
                    //HttpContext.Current.Response.ContentType = "application/pdf";
                    //HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + filefullpath);
                    //HttpContext.Current.Response.OutputStream.Write(bytes, 0, bytes.Length);
                    //Response.Flush(); // Sends all currently buffered output to the client.
                    //HttpContext.Current.ApplicationInstance.CompleteRequest();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + filefullpath);
                    Response.OutputStream.Write(bytes, 0, bytes.Length);
                    Response.Flush();
                    Response.End();

                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

