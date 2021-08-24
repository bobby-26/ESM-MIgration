using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PlannedMaintenanceSubWorkOrderList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarRA = new PhoenixToolbar();
            toolbarRA.AddFontAwesomeButton("", "Create Requisition", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
            MenuRA.AccessRights = this.ViewState;
            MenuRA.MenuList = toolbarRA.Show();

            if (!IsPostBack)
            {

                gvWorkOrder.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["groupId"] = "";
                ViewState["WONUMBER"] = string.Empty;
                ViewState["EDIT"] = "0";
                ViewState["CODE"] = string.Empty;
                ViewState["ALLOWEDIT"] = "1";
                if (Request.QueryString["groupId"] != null)
                    ViewState["groupId"] = Request.QueryString["groupId"].ToString();
                if (Request.QueryString["WONUMBER"] != null)
                    ViewState["WONUMBER"] = Request.QueryString["WONUMBER"].ToString();
                ViewState["vslid"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                if (Request.QueryString["vslid"] != null)
                {
                    ViewState["vslid"] = Request.QueryString["vslid"];
                }
                ViewState["DWPID"] = Request.QueryString["wopid"] ?? string.Empty;
            }
            ViewMenu();
            ResetMenu();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ViewMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceSubWorkOrderList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvWorkOrder')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("javascript: $modalWindow.modalWindowUrl='" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderGroupToolBoxMeet.aspx?groupId=" + ViewState["groupId"] + "&WONUMBER=" + ViewState["WONUMBER"] + "&wopid="+ ViewState["DWPID"] + "'; showDialog('Toolbox Meet');", "Toolbox Meet", "<i class=\"fas fa-toolbox\"></i>", "TOOLBOX");
        toolbar.AddImageButton("", "Print Toolbox", "pdf.png", "TOOLBOXPRINT");
        //javascript: openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentRiskAssessmentList.aspx?ComponentId=" + drv["FLDCOMPONENTID"].ToString() + "&WorkorderId=" + drv["FLDWORKORDERID"].ToString() + "&GroupId=" + ViewState["groupId"] + "');
        if (ViewState["EDIT"].ToString() == "0" && ViewState["ALLOWEDIT"].ToString() == "1")
        {
            toolbar.AddButton("Edit", "Edit", ToolBarDirection.Right);
        }
        if (ViewState["EDIT"].ToString() == "1")
        {
            if (",ISS,POP,".Contains("," + ViewState["CODE"].ToString() + ","))
                toolbar.AddFontAwesomeButton("javascript:openNewWindow('md','Jobs','" + Session["sitepath"] + "/Dashboard/DashboardTechnicalPmsMaitenanceDue.aspx?frm=wojob&groupId=" + ViewState["groupId"] + "');", "Create WO", "<i class=\"fas fa-plus-circle\"></i>", "CREATE");
            toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        }
        //toolbarNoonReporttap.AddButton("Print Toolbox Meeting Form", "TOOLBOX", ToolBarDirection.Right);
        MenuDivWorkOrder.AccessRights = this.ViewState;
        MenuDivWorkOrder.MenuList = toolbar.Show();
        RadScriptManager.GetCurrent(this).RegisterPostBackControl(MenuDivWorkOrder);
    }

    protected void MenuSubWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("WOORDER"))
            {
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceWorkOrderGroupList.aspx?groupId=" + ViewState["groupId"] + "&FromJob=1");
            }
            if (CommandName.ToUpper().Equals("JOBS"))
            {
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceSubWorkOrderList.aspx?groupId=" + ViewState["groupId"]);
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDPLANNINGDUEDATE", "FLDWORKORDERSTATUS" };
            string[] alCaptions = { "Comp. No", "Comp. Name", "Job No", "Job Title", "Due Date", "Status" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds;

            ds = PhoenixPlannedMaintenanceWorkOrderGroup.SubWorkOrderList(new Guid(ViewState["groupId"].ToString()), int.Parse(ViewState["vslid"].ToString()), sortexpression, sortdirection,
                             1, iRowCount, ref iRowCount, ref iTotalPageCount);

            General.ShowExcel("Work Order", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuDivWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("GENERATE"))
            {
                PhoenixPlannedMaintenanceWorkOrderGroup.GroupWoGenerate(new Guid(ViewState["groupId"].ToString()));
                gvWorkOrder.Rebind();
                RadNotification1.Show("Work order issued");
            }
            else if (CommandName.ToUpper().Equals("TOOLBOXPRINT"))
            {
                ConvertToPdf(PrepareHtmlDoc(null));
            }
            else if (CommandName.ToUpper().Equals("EDIT"))
            {
                ViewState["EDIT"] = "1";
                ViewMenu();
                gvWorkOrder.Rebind();
            }
            else if (CommandName.ToUpper().Equals("CANCEL"))
            {
                ViewState["EDIT"] = "0";
                ViewMenu();
                gvWorkOrder.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
    private string PrepareHtmlDoc(string toolboxid)
    {
        DataSet dsPDF = PhoenixPlannedMaintenanceWorkOrderGroup.SubWorkOrderToolboxMeetingForm(new Guid(ViewState["groupId"].ToString()), General.GetNullableGuid(toolboxid));

        StringBuilder DsHtmlcontent = new StringBuilder();

        if (dsPDF.Tables[0].Rows.Count > 0)
        {
            DataTable main = dsPDF.Tables[0];
            DataRow maindr = dsPDF.Tables[1].Rows[0];
            DsHtmlcontent.Append("<html>");
            DsHtmlcontent.Append("<table style=\"width:100%; font-size: 10px;\"><tr><td>" + PhoenixSecurityContext.CurrentSecurityContext.VesselName + "</td>");
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
                DsHtmlcontent.Append("<td colspan=\"2\">" + dr["FLDJOBDETAILCHECK"].ToString() + "</td>");
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
                DsHtmlcontent.Append("<td>" + dr["FLDATTREQUIRED"].ToString() + "</td>");
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
            DsHtmlcontent.Append("<td colspan=\"3\">Person in Charge: " + main.Rows[0]["FLDPIC"].ToString() + "</td>");
            DsHtmlcontent.Append("<td colspan=\"3\">Other Team Members: " + main.Rows[0]["FLDOTHER"].ToString().Trim(',') + "</td>");
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
    private void BindData()
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

            DataSet ds;

            ds = PhoenixPlannedMaintenanceWorkOrderGroup.SubWorkOrderList(new Guid(ViewState["groupId"].ToString()), int.Parse(ViewState["vslid"].ToString()), sortexpression, sortdirection,
                            gvWorkOrder.CurrentPageIndex + 1, gvWorkOrder.PageSize, ref iRowCount, ref iTotalPageCount);
            General.SetPrintOptions("gvWorkOrder", "Work Order", alCaptions, alColumns, ds);

            gvWorkOrder.DataSource = ds;
            gvWorkOrder.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;

            BindWorkOrder(ds.Tables[1]);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvWorkOrder_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWorkOrder_SortCommand(object sender, GridSortCommandEventArgs e)
    {

    }
    protected void gvWorkOrder_ItemCommand1(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                RadLabel subJobid = item.FindControl("lblSubJobId") as RadLabel;
                PhoenixPlannedMaintenanceWorkOrderGroup.SubjobDelete(new Guid(subJobid.Text), int.Parse(ViewState["vslid"].ToString()));
                if (gvWorkOrder.Items.Count <= 1)
                {
                    Response.Redirect("../PlannedMaintenance/PlannedMaintenanceWorkOrderGroupList.aspx?groupId=" + ViewState["groupId"] + "&FromJob=1");
                }
                else
                {
                    gvWorkOrder.DataSource = null;
                    gvWorkOrder.Rebind();
                }
            }
            else if (e.CommandName.ToUpper().Equals("RADELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                RadLabel id = item.FindControl("lblWorkorderId") as RadLabel;
                PhoenixPlannedMaintenanceWorkOrderGroup.DeleteWorkorderRA(new Guid(id.Text), int.Parse(ViewState["vslid"].ToString()));
                gvWorkOrder.DataSource = null;
                gvWorkOrder.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("RAEDIT"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                RadLabel riskId = item.FindControl("lblRaId") as RadLabel;
                RadLabel workorderId = item.FindControl("lblWorkorderId") as RadLabel;
                string raStatus;
                string RAcreatedIn;
                if (riskId.Text != "")
                {
                    DataTable dt = PhoenixPlannedMaintenanceWorkOrderGroup.GetRaStatus(new Guid(riskId.Text));
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        raStatus = dr["FLDSTATUS"].ToString();
                        RAcreatedIn = dr["FLDISCREATEDBYOFFICE"].ToString();
                        //RadWindow_NavigateUrl.NavigateUrl = "../Inspection/InspectionRAMachineryExtn.aspx?FromWorkorderGroup=1&status=" + raStatus + "&machineryid=" + riskId.Text + "&RaCreatedIn=" + RAcreatedIn + "&PAGENUMBER=" + ViewState["PAGENUMBER"] + "&WORKORDERID=" + workorderId.Text + "&WORKORDERGROUPID=" + ViewState["groupId"];
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                "BookMarkScript", "$modalWindow.modalWindowUrl='../Inspection/InspectionRAMachineryExtn.aspx?FromWorkorderGroup=1&status=" + raStatus + "&machineryid=" + riskId.Text + "&RaCreatedIn=" + RAcreatedIn + "&PAGENUMBER=" + ViewState["PAGENUMBER"] + "&WORKORDERID=" + workorderId.Text + "&WORKORDERGROUPID=" + ViewState["groupId"] + "';showDialog('RA')", true);
                        //Response.Redirect("../Inspection/InspectionRAMachineryExtn.aspx?FromWorkorderGroup=1&status=" + raStatus + "&machineryid=" + riskId.Text + "&RaCreatedIn=" + RAcreatedIn + "&PAGENUMBER=" + ViewState["PAGENUMBER"] + "&WORKORDERID=" + workorderId.Text + "&WORKORDERGROUPID=" + ViewState["groupId"], false);
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
    protected void gvWorkOrder_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                LinkButton cmddelete = (LinkButton)e.Item.FindControl("cmdDelete");
                if (cmddelete != null)
                {
                    cmddelete.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
                    if (ViewState["EDIT"].ToString() == "0")
                        cmddelete.Visible = false;
                    else
                        cmddelete.Visible = SessionUtil.CanAccess(this.ViewState, cmddelete.CommandName);
                }
                LinkButton jd = (LinkButton)e.Item.FindControl("lnkJobDetaiil");
                if (jd != null)
                {
                    jd.Visible = SessionUtil.CanAccess(this.ViewState, jd.CommandName);
                    if (Request.QueryString["vslid"] != null)
                        jd.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceJobDetailForWorkOrder.aspx?JOBID=" + drv["FLDJOBID"] + "&vesselid=" + Request.QueryString["vslid"] + "'); ");
                    else
                        jd.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceJobDetailForWorkOrder.aspx?JOBID=" + drv["FLDJOBID"] + "'); ");

                }
                LinkButton lw = (LinkButton)e.Item.FindControl("lnkWorkorderName");
                if (lw != null)
                {
                    lw.Visible = SessionUtil.CanAccess(this.ViewState, lw.CommandName);
                    string cjid = drv["FLDCOMPONENTJOBID"].ToString();
                    if (General.GetNullableGuid(cjid).HasValue && General.GetNullableGuid(cjid).Value != Guid.Empty)
                    {
                        if (Request.QueryString["vslid"] != null)
                            lw.Attributes.Add("onclick", "javascript:openNewWindow('PPLIST', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJob.aspx?COMPONENTJOBID=" + drv["FLDCOMPONENTJOBID"] + "&COMPONENTID=" + drv["FLDCOMPONENTID"] + "&Cancelledjob=0&disback=1&vesselid=" + Request.QueryString["vslid"] + "'); ");
                        else
                            lw.Attributes.Add("onclick", "javascript:openNewWindow('PPLIST', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJob.aspx?COMPONENTJOBID=" + drv["FLDCOMPONENTJOBID"] + "&COMPONENTID=" + drv["FLDCOMPONENTID"] + "&Cancelledjob=0&disback=1'); ");
                    }
                    else
                    {
                        if (Request.QueryString["vslid"] != null)
                            lw.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderDetail.aspx?WORKORDERID=" + drv["FLDWORKORDERID"].ToString() + "&vesselid=" + Request.QueryString["vslid"] + "','','1200','600');return false");
                        else
                            lw.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderDetail.aspx?WORKORDERID=" + drv["FLDWORKORDERID"].ToString() + "','','1200','600');return false");
                    }
                }
                
                LinkButton temp = (LinkButton)e.Item.FindControl("lnkTemplates");
                if (temp != null)
                {
                    temp.Visible = SessionUtil.CanAccess(this.ViewState, temp.CommandName);
                    if (drv["FLDTEMPLATESMAPPED"].ToString() == "0")
                    {
                        temp.Enabled = false;
                        temp.Attributes["style"] = "color:Black !important;";
                    }
                    else
                    {
                        if (Request.QueryString["vslid"] != null)
                            temp.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderReportComponent.aspx?Workorderid=" + drv["FLDWORKORDERID"] + "&vesselid=" + Request.QueryString["vslid"] + "'); ");
                        else
                            temp.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderReportComponent.aspx?Workorderid=" + drv["FLDWORKORDERID"] + "'); ");
                    }
                }
                LinkButton pq = (LinkButton)e.Item.FindControl("lnkParts");
                if (pq != null)
                {
                    if (drv["FLDCATEGORYLEVEL"].ToString() == "2")
                    {
                        pq.Text = "NA";
                        pq.Attributes["style"] = "color:Black !important;";
                        pq.Enabled = false;
                    }
                    else
                    {
                        pq.Visible = SessionUtil.CanAccess(this.ViewState, pq.CommandName);
                        if (Request.QueryString["vslid"] != null)
                            pq.Attributes.Add("onclick", "javascript:$modalWindow.modalWindowUrl='" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderPartsRequired.aspx?WORKORDERID=" + drv["FLDWORKORDERID"] + "&COMPONENTID=" + drv["FLDCOMPONENTID"] + "&COMPNO=" + drv["FLDCOMPONENTNUMBER"] + "&COMPNAME=" + drv["FLDCOMPONENTNAME"] + "&WORKORDERGROUPID=" + ViewState["groupId"] + "&vesselid=" + Request.QueryString["vslid"] + "'; showDialog('Add Parts');");
                        else
                            pq.Attributes.Add("onclick", "javascript:$modalWindow.modalWindowUrl='" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderPartsRequired.aspx?WORKORDERID=" + drv["FLDWORKORDERID"] + "&COMPONENTID=" + drv["FLDCOMPONENTID"] + "&COMPNO=" + drv["FLDCOMPONENTNUMBER"] + "&COMPNAME=" + drv["FLDCOMPONENTNAME"] + "&WORKORDERGROUPID=" + ViewState["groupId"] + "'; showDialog('Add Parts');");
                    }
                }
                LinkButton jha = (LinkButton)e.Item.FindControl("lnkJHA");
                if (jha != null)
                {
                    jha.Visible = SessionUtil.CanAccess(this.ViewState, jha.CommandName);
                    if (Request.QueryString["vslid"] != null)
                        jha.Attributes.Add("onclick", "javascript:$modalWindow.modalWindowUrl='" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceJobsJHAList.aspx?WORKORDERID=" + drv["FLDWORKORDERID"].ToString() + "&WORKORDERGROUPID=" + ViewState["groupId"] + "&vesselid=" + Request.QueryString["vslid"] + "'; showDialog('JHA');");                    
                    else
                        jha.Attributes.Add("onclick", "javascript:$modalWindow.modalWindowUrl='" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceJobsJHAList.aspx?WORKORDERID=" + drv["FLDWORKORDERID"].ToString() + "&WORKORDERGROUPID=" + ViewState["groupId"] + "'; showDialog('JHA');");
                }
                LinkButton Ptw = (LinkButton)e.Item.FindControl("lnkPTW");
                if (Ptw != null)
                {
                    Ptw.Visible = SessionUtil.CanAccess(this.ViewState, Ptw.CommandName);
                    if (Request.QueryString["vslid"] != null)
                        Ptw.Attributes.Add("onclick", "javascript:$modalWindow.modalWindowUrl='" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceJobPtwList.aspx?JobId=" + drv["FLDJOBID"].ToString() + "&workorderId=" + drv["FLDWORKORDERID"] + "&WORKORDERGROUPID=" + ViewState["groupId"] + "&vesselid=" + Request.QueryString["vslid"] + "'; showDialog('PTW');");                    
                    else
                        Ptw.Attributes.Add("onclick", "javascript:$modalWindow.modalWindowUrl='" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceJobPtwList.aspx?JobId=" + drv["FLDJOBID"].ToString() + "&workorderId=" + drv["FLDWORKORDERID"] + "&WORKORDERGROUPID=" + ViewState["groupId"] + "'; showDialog('PTW');");
                }

                LinkButton riskCreate = (LinkButton)e.Item.FindControl("lnkRiskCreate");
                //LinkButton riskView = (LinkButton)e.Item.FindControl("lnkRiskView");
                LinkButton riskEdit = (LinkButton)e.Item.FindControl("lnkRaEdit");
                //Label separator1 = (Label)e.Item.FindControl("lblSeparator1");
                //Label separator2 = (Label)e.Item.FindControl("lblSeparator2");
                LinkButton raDelete = (LinkButton)e.Item.FindControl("cmdRADelete");
                if (riskCreate != null)
                {
                    if (drv["FLDRAREQUIRED"].ToString() == "1" && (string.IsNullOrEmpty(drv["FLDRAID"].ToString()) || drv["FLDRAREJECTED"].ToString() == "1"))
                    {
                        riskCreate.Visible = true;
                        if (Request.QueryString["vslid"] != null)
                            riskCreate.Attributes.Add("onclick", "javascript:$modalWindow.modalWindowUrl='" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentRiskAssessmentList.aspx?ComponentId=" + drv["FLDCOMPONENTID"].ToString() + "&WorkorderId=" + drv["FLDWORKORDERID"].ToString() + "&GroupId=" + ViewState["groupId"] + "&inlp=1&vesselid=" + Request.QueryString["vslid"] + "';showDialog('Map RA');");
                        else
                            riskCreate.Attributes.Add("onclick", "javascript:$modalWindow.modalWindowUrl='" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentRiskAssessmentList.aspx?ComponentId=" + drv["FLDCOMPONENTID"].ToString() + "&WorkorderId=" + drv["FLDWORKORDERID"].ToString() + "&GroupId=" + ViewState["groupId"] + "&inlp=1';showDialog('Map RA');");
                        riskCreate.Visible = SessionUtil.CanAccess(this.ViewState, riskCreate.CommandName);
                    }
                    if (drv["FLDRAREQUIRED"].ToString() == "0")
                    {
                        riskCreate.Visible = true;
                        riskCreate.Text = "NA";
                        riskCreate.Attributes["style"] = "color:Black !important;";
                        riskCreate.Enabled = false;
                    }

                }
                if (riskEdit != null)
                {
                    if (!string.IsNullOrEmpty(drv["FLDRAID"].ToString()))
                    {
                        //riskView.Visible = true;
                        //riskView.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAMACHINERYNEW&machineryid=" + drv["FLDRAID"].ToString() + "&showmenu=0&showexcel=NO');");
                        //riskView.Visible = SessionUtil.CanAccess(this.ViewState, riskView.CommandName);
                        if (riskEdit != null)
                        {
                            raDelete.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event, 'Are you sure want to remove'); return false;");
                            raDelete.Visible = SessionUtil.CanAccess(this.ViewState, riskEdit.CommandName);
                            //separator1.Visible = true;
                            //separator2.Visible = true;
                            riskEdit.Visible = SessionUtil.CanAccess(this.ViewState, riskEdit.CommandName);
                        }
                    }
                }
                ImageButton lnkwaive = (ImageButton)e.Item.FindControl("lnkPtwWaive");
                if (lnkwaive != null)
                {
                    lnkwaive.Visible = false;
                    if (Request.QueryString["vslid"] != null)
                        lnkwaive.Attributes.Add("onclick", "javascript:$modalWindow.modalWindowUrl='" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceSubWorkOrderWaiverDetail.aspx?WORKORDERID=" + drv["FLDWORKORDERID"] + "&WorkGroupId=" + ViewState["groupId"].ToString() + "&vesselid=" + Request.QueryString["vslid"] + "';showDialog('Waive');");
                    else
                        lnkwaive.Attributes.Add("onclick", "javascript:$modalWindow.modalWindowUrl='" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceSubWorkOrderWaiverDetail.aspx?WORKORDERID=" + drv["FLDWORKORDERID"] + "&WorkGroupId=" + ViewState["groupId"].ToString() + "';showDialog('Waive');");
                    if (drv["FLDISRAMET"].ToString().ToLower() == "no" || drv["FLDISPTWMET"].ToString().ToLower() == "no" 
                        || drv["FLDISSPARESMET"].ToString().ToLower() == "no")
                        lnkwaive.Visible = SessionUtil.CanAccess(this.ViewState, lnkwaive.CommandName);
                }
                TableCell cell = item["RAMET"];
                if (drv["FLDISRAMET"].ToString().ToLower() == "yes")
                    cell.BackColor = System.Drawing.Color.Green;
                else if (drv["FLDISRAMET"].ToString().ToLower() == "no")
                    cell.BackColor = System.Drawing.Color.Red;

                //cell = item["PTWREQ"];
                //if (drv["FLDISPTWREQ"].ToString().ToLower() == "yes")
                //    cell.BackColor = Color.Green;
                //else if (drv["FLDISPTWREQ"].ToString().ToLower() == "no")
                //    cell.BackColor = Color.Red;

                cell = item["PTWMET"];
                if (drv["FLDISPTWMET"].ToString().ToLower() == "yes")
                    cell.BackColor = System.Drawing.Color.Green;
                else if (drv["FLDISPTWMET"].ToString().ToLower() == "no")
                    cell.BackColor = System.Drawing.Color.Red;

                //cell = item["SPARESREQ"];
                //if (drv["FLDISSPARESREQ"].ToString().ToLower() == "yes")
                //    cell.BackColor = Color.Green;
                //else if (drv["FLDISSPARESREQ"].ToString().ToLower() == "no")
                //    cell.BackColor = Color.Red;

                cell = item["SPARESMET"];
                if (drv["FLDISSPARESMET"].ToString().ToLower() == "yes")
                    cell.BackColor = System.Drawing.Color.Green;
                else if (drv["FLDISSPARESMET"].ToString().ToLower() == "no")
                    cell.BackColor = System.Drawing.Color.Red;

                LinkButton lnkAttachmentReqd = (LinkButton)e.Item.FindControl("lnkAttachmentReqd");
                if (lnkAttachmentReqd != null)
                {
                    if (drv["FLDATTACHMENTREQYN"].ToString() == "1")
                    {
                        lnkAttachmentReqd.Text = "Yes";

                        lnkAttachmentReqd.ToolTip = drv["FLDATTINSTRUCTIONS"].ToString();
                        RadToolTipManager1.TargetControls.Add(lnkAttachmentReqd.ID);
                    }
                    else
                    {
                        lnkAttachmentReqd.Enabled = false;
                        lnkAttachmentReqd.Attributes["style"] = "color:Black !important;";
                        lnkAttachmentReqd.Text = "No";
                    }
                }
                LinkButton lnkParam = (LinkButton)e.Item.FindControl("lnkParam");
                if (lnkParam != null)
                {
                    if (drv["FLDISJOBPARAMETER"].ToString() == "1")
                    {
                        if (Request.QueryString["vslid"] != null)
                            lnkParam.Attributes.Add("onclick", "javascript:$modalWindow.modalWindowUrl='" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceJobParameterList.aspx?WORKORDERID=" + drv["FLDWORKORDERID"] + "&vesselid=" + Request.QueryString["vslid"] + "';showDialog('Parameter');");
                        else
                            lnkParam.Attributes.Add("onclick", "javascript:$modalWindow.modalWindowUrl='" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceJobParameterList.aspx?WORKORDERID=" + drv["FLDWORKORDERID"] + "';showDialog('Parameter');");
                    }
                    else
                    {
                        lnkParam.Text = "NA";
                        lnkParam.Attributes["style"] = "color:Black !important;";
                        lnkParam.Enabled = false;
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
    protected void gvWorkOrder_PreRender(object sender, EventArgs e)
    {
        foreach (GridDataItem dataItem in gvWorkOrder.MasterTableView.Items)
        {
            //if (dataItem["Column2"].Text == dataItem["Column3"].Text)
            //{
            //    dataItem["Column2"].ColumnSpan = 2;
            //    dataItem["Column3"].Visible = false;
            //}

            int previousItemIndex = dataItem.ItemIndex - 1;
            if (previousItemIndex >= 0)
            {
                string current = ((RadLabel)dataItem["FLDRAID"].FindControl("lblRaId")).Text;
                string prev = ((RadLabel)dataItem.OwnerTableView.Items[previousItemIndex]["FLDRAID"].FindControl("lblRaId")).Text;
                LinkButton ralink = ((LinkButton)dataItem["FLDRAID"].FindControl("lnkRiskCreate")); 
                if (ralink.Visible && ralink.Text.ToUpper() != "NA") continue;
                if (current == prev)
                {
                    int tempindex = previousItemIndex;
                    //dataItem.OwnerTableView.Items[previousItemIndex]["FLDRAID"].RowSpan = 2;                    
                    dataItem["FLDRAID"].Visible = false;
                    dataItem["RAREQ"].Visible = false;
                    dataItem["RAMET"].Visible = false;
                    int cnt = 1;
                    while(tempindex >= 0)
                    {
                        string tempprev = ((RadLabel)dataItem.OwnerTableView.Items[tempindex]["FLDRAID"].FindControl("lblRaId")).Text;
                        if (current != tempprev) break;
                        cnt++;
                        tempindex--;
                    }
                    dataItem.OwnerTableView.Items[tempindex + 1]["FLDRAID"].RowSpan = cnt;
                    dataItem.OwnerTableView.Items[tempindex + 1]["RAREQ"].RowSpan = cnt;
                    dataItem.OwnerTableView.Items[tempindex + 1]["RAMET"].RowSpan = cnt;
                }                
            }
        }
    }
    private void BindWorkOrder(DataTable dt)
    {
        ClearWorkOrder();
        DataRow dr = dt.Rows[0];
        lblworkorderNo.Text = lblworkorderNo.Text + " " + dr["FLDWORKORDERNUMBER"].ToString();
        lblCategory.Text = lblCategory.Text + " " + dr["FLDJOBCATEGORY"].ToString();
        lblPlanDate.Text = lblPlanDate.Text + " " + General.GetDateTimeToString(dr["FLDPLANNINGDUEDATE"].ToString());
        lblDuration.Text = lblDuration.Text + " " + dr["FLDPLANNINGESTIMETDURATION"].ToString();
        lblResponsible.Text = lblResponsible.Text + " " + dr["FLDDISCIPLINENAME"].ToString();
        lblStatus.Text = lblStatus.Text + " " + dr["FLDSTATUS"].ToString();
        lblRoutine.Text = lblRoutine.Text + " " + (dr["FLDISUNPLANNEDJOB"].ToString().Equals("1") ? "No" : "Yes");
        ViewState["WONUMBER"] = dr["FLDWORKORDERNUMBER"].ToString();
        ViewState["CODE"] = dr["FLDSTATUSCODE"].ToString();
        ViewState["ALLOWEDIT"] = dr["FLDALLOWEDIT"].ToString();
        ViewMenu();
    }
    private void ClearWorkOrder()
    {
        lblworkorderNo.Text = "Work order No :";
        lblCategory.Text = "Category :";
        lblPlanDate.Text = "Plan Date :";
        lblDuration.Text = "Duration :";
        lblResponsible.Text = "Responsibility :";
        lblStatus.Text = "Status :";
        lblRoutine.Text = "Routine :";
    }
    private void ResetMenu()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("List", "WOORDER");
        toolbarmain.AddButton("Jobs", "JOBS");

        MenuSubWorkOrder.AccessRights = this.ViewState;
        MenuSubWorkOrder.MenuList = toolbarmain.Show();
        //MenuWorkOrder.SetTrigger(pnlComponent);
        MenuSubWorkOrder.SelectedMenuIndex = 1;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvWorkOrder.Rebind();
    }

    protected void gvPartsSummary_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixPlannedMaintenanceWorkOrderGroup.WorkorderGroupPartsRequiredSummary(new Guid(ViewState["groupId"].ToString()), int.Parse(ViewState["vslid"].ToString()));
        gvPartsSummary.DataSource = dt;

    }

    protected void MenuRA_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("ADD"))
        {
            StringBuilder strSpare = new StringBuilder();
            StringBuilder strQty = new StringBuilder();
            if (gvPartsSummary.SelectedItems.Count > 0)
            {
                foreach (GridDataItem gv in gvPartsSummary.SelectedItems)
                {
                    RadLabel lblSpareItem = (RadLabel)gv.FindControl("lblspareItemId");
                    strSpare.Append(lblSpareItem.Text + ",");

                    RadLabel lblQty = (RadLabel)gv.FindControl("lblshortageQty");
                    strQty.Append(lblQty.Text + ",");

                }

                PhoenixPlannedMaintenanceWorkOrderGroup.RequisitionCreate(strSpare.ToString(), strQty.ToString(), new Guid(ViewState["groupId"].ToString()), int.Parse(ViewState["vslid"].ToString()));

                RadNotification1.Show("Requisition Created.");
                gvPartsSummary.Rebind();
            }
            else
            {
                ucError.ErrorMessage = "No parts are selected for creating requistion. <br/> To add required parts click on 'Show' under 'Spare' Column <br/> Note: Category 4 jobs spares is NA";
                ucError.Visible = true;
            }
        }

    }

    protected void gvPartsSummary_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            GridDataItem item = (GridDataItem)e.Item;
            LinkButton lnkSpare = (LinkButton)e.Item.FindControl("lnkPartNumber");
            if (lnkSpare != null)
            {
                lnkSpare.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inventory/InventorySpareItemGeneral.aspx?SPAREITEMID="+drv["FLDSPAREITEMID"] +"'); return false;");
            }

            if (General.GetNullableGuid(drv["FLDORDERID"].ToString()) != null)
            {
                item["ClientSelectColumn"].Enabled = false;
                item.SelectableMode = GridItemSelectableMode.None;
            }

        }
    }

    protected void gvPartsSummary_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "REQUISITION")
        {
            GridDataItem item = (GridDataItem)e.Item;
            LinkButton lnkFormNo = (LinkButton)item.FindControl("lnkFormNo");

            NameValueCollection criteria = new NameValueCollection();
            criteria.Clear();
            criteria.Add("ucVessel", item.GetDataKeyValue("FLDVESSELID").ToString());
            criteria.Add("ddlStockType", "SPARE");
            criteria.Add("txtNumber", lnkFormNo.Text);
            Filter.CurrentOrderFormFilterCriteria = criteria;

            Filter.CurrentPurchaseDashboardCode = null;

            string script = "top.openNewWindow('detail', 'Requisition', 'Purchase/PurchaseForm.aspx');";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
        }
    }
}
