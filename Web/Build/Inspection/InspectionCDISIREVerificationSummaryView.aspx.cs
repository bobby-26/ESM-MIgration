using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using SouthNests.Phoenix.Integration;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
using Newtonsoft.Json;
using System.Configuration;
using SouthNests.Phoenix.Registers;
using System.Collections;


public partial class Inspection_InspectionCDISIREVerificationSummaryView : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["CATEGORYID"] = "";
            ViewState["NOOFCOLUMNS"] = "";
            ViewState["INSPECTIONID"] = "";
            ViewState["COUNTTYPE"] = null;


            if (Request.QueryString["categoryid"] != null)
                ViewState["CATEGORYID"] = Request.QueryString["categoryid"].ToString();

            if (Request.QueryString["inspectionid"] != null)
                ViewState["INSPECTIONID"] = Request.QueryString["inspectionid"].ToString();

            if (Request.QueryString["counttype"] != null)
                ViewState["COUNTTYPE"] = Request.QueryString["counttype"].ToString();

            MenuPhoenixQuery.Visible = false;

            if (Request.QueryString["categoryid"] != null)
            {
                BindData();
                MenuPhoenixQuery.Visible = true;
            }
            //BindCompany();
        }
        setMenu();
        //if (Request.QueryString["categoryid"] != null || ViewState["CONTENTID"] != null)
        //{
        //    BindData();
        //}

    }
    //protected void BindCompany()
    //{
    //    DataSet ds = PhoenixInspectionOilMajorComany.ListOilMajorCompany(1, null);

    //    ddlCompany.DataSource = ds.Tables[0];
    //    ddlCompany.DataTextField = "FLDOILMAJORCOMPANYNAME";
    //    ddlCompany.DataValueField = "FLDOILMAJORCOMPANYID";
    //    ddlCompany.DataBind();
    //    ddlCompany.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    //}
    private void setMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
        {
            toolbar.AddFontAwesomeButton("../Inspection/InspectionCDISIREMatrixCategoryContentList.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        }
        //toolbar.AddImageLink("javascript:parent.Openpopup('Filter','','../Inspection/InspectionCDISIREMatrixContentAdd.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&noofcolumns=" + General.GetNullableInteger(ViewState["NOOFCOLUMNS"].ToString()) + "'); return false;", "Add", "add.png", "ADD");
        //toolbar.AddImageLink("javascript:Openpopup('codehelp1','','InspectionIncidentAdd.aspx');return true;", "Add New", "add.png", "ADD");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionCDISIREVerificationSummaryView.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
        {
            toolbar.AddFontAwesomeButton("../Inspection/InspectionCDISIREMatrixCategoryContentList.aspx", "Publish", "<i class=\"fa fa-check-square\"></i>", "PUBLISH");
        }
        toolbar.AddFontAwesomeButton("../Inspection/InspectionCDISIREMatrixCategoryContentList.aspx", "Revisions", "<i class=\"fas fa-receipt\"></i>", "REVISIONS");
        //toolbar.AddImageLink("javascript:parent.openNewWindow('Filter','','Inspection/InspectionCDISIREVerificationSummary.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&inspectionid=" + General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()) + "'); return false;", "Verification Summary", "checklist.png", "SUMMARY");
        //toolbar.AddImageLink("../Inspection/InspectionCDISIREVerificationSummary.aspx", "Verification Summary", "<i class=\"fas fa-receipt\"></i>", "SUMMARY");
        MenuPhoenixQuery.AccessRights = this.ViewState;
        MenuPhoenixQuery.MenuList = toolbar.Show();
        MenuPhoenixQuery.Visible = true;
    }

    protected void MenuPhoenixQuery_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("ADD"))
        {
            //String scriptpopup = String.Format(
            //   "javascript:Openpopup('codehelp1', '', '../Inspection/InspectionCDISIREMatrixContentAdd.aspx?categoryid=" + General.GetNullableGuid(lblSelectedNode.Text) + "&noofcolumns=" + txtnoofcolumns.Text + "');");
            //ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            Response.Redirect("../Inspection/InspectionCDISIREMatrixContentAdd.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&noofcolumns=" + General.GetNullableInteger(ViewState["NOOFCOLUMNS"].ToString()) + "&inspectionid=" + General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()) + "&action=add");

        }

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }

        if (CommandName.ToUpper().Equals("PUBLISH"))
        {
        }

        if (CommandName.ToUpper().Equals("REVISIONS"))
        {
        }
    }

    protected void ShowExcel()
    {

        DataTable dt = new DataTable();
        dt = PhoenixInspectionCDISIREMatrix.EditCDISIREMatrixColumnHeaders(General.GetNullableGuid(ViewState["CATEGORYID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        string captions = "";
        string columns = "";
        string columnname = "FLDCOLUMN";
        int noofcolumns = int.Parse(dt.Rows[0]["FLDNOOFCOLUMNS"].ToString());

        if (dt.Rows.Count > 0)
        {
            for (int i = 1; i <= noofcolumns; i++)
            {
                string colheader = "FLDCOLUMNTEXT";

                foreach (DataRow dr in dt.Rows)
                {
                    colheader = colheader + i.ToString();
                    if (captions.Equals(""))
                        captions = dr[colheader].ToString();
                    else
                        captions = captions + "," + dr[colheader].ToString();

                    if (columns.Equals(""))
                        columns = columnname + i.ToString();
                    else
                        columns = columns + "," + columnname + i.ToString();
                }
            }
        }

        columns = columns + ",FLDPROCEDURES,FLDOBJECTIVEEVIDENCE,FLDDEPARTMENTNAMELIST,FLDISEVIDENCEREQD,FLDOFFICEREMARKS,FLDREMARKS";
        captions = captions + ",Procedures, Objective Evidence,Responsibility, Evidence Required, Office Remarks, Onboard Remarks";

        string[] alColumns = columns.Split(',');
        string[] alCaptions = captions.Split(',');


        DataTable dtcolumn = new DataTable();

        dtcolumn = PhoenixInspectionCDISIREMatrix.CDISIREMatrixSummaryVerificationView(General.GetNullableGuid(ViewState["CATEGORYID"].ToString()), null,PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableInteger(ViewState["COUNTTYPE"].ToString()));

        if (dtcolumn.Rows.Count > 0)
        {
            ShowQuery(General.GetNullableGuid(ViewState["CATEGORYID"].ToString()), dt, dtcolumn);
        }
        else
        {
            gvCDISIREMatrix.Visible = false;
            //dtcolumn.Rows.Clear();
            //DataTable dt1 = dtcolumn;
            //ShowNoRecordsFound(dt1, gvCDISIREMatrix);
        }
        ShowExcel("Summary Verification View", dtcolumn, alColumns, alCaptions, null, null);
    }
    public static void ShowExcel(string strHeading, DataTable dt, string[] alColumns, string[] alCaptions, int? strSortDirection, string strSortExpression)
    {

        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + (string.IsNullOrEmpty(strHeading) ? "Attachment" : strHeading.Replace(" ", "_")) + ".xls");
        HttpContext.Current.Response.ContentType = "application/vnd.msexcel";
        HttpContext.Current.Response.Write("<style>.text{ mso-number-format:\"\\@\";}</style>");
        HttpContext.Current.Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");

        HttpContext.Current.Response.Write("<tr>");
        HttpContext.Current.Response.Write("<td ><img src='http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        HttpContext.Current.Response.Write("<td colspan='" + (alColumns.Length - 1).ToString() + "'><h3>&nbsp&nbsp&nbsp&nbsp " + strHeading + "-" + dt.Rows[0]["FLDCATEGORYNAME"].ToString() + "</h3></td>");
        HttpContext.Current.Response.Write("</tr>");
        HttpContext.Current.Response.Write("</TABLE>");
        HttpContext.Current.Response.Write("<br />");
        HttpContext.Current.Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        HttpContext.Current.Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            HttpContext.Current.Response.Write("<td width='20%'>");
            HttpContext.Current.Response.Write("<b>" + alCaptions[i] + "</b>");
            HttpContext.Current.Response.Write("</td>");
        }
        HttpContext.Current.Response.Write("</tr>");
        foreach (DataRow dr in dt.Rows)
        {
            HttpContext.Current.Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                HttpContext.Current.Response.Write(dr[alColumns[i]].GetType().Equals(typeof(string)) ? "<td  class='text'>" : "<td>");
                HttpContext.Current.Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]].ToString());
                HttpContext.Current.Response.Write("</td>");
            }
            HttpContext.Current.Response.Write("</tr>");
        }
        HttpContext.Current.Response.Write("</TABLE>");
        HttpContext.Current.Response.Write("<br/>");
        HttpContext.Current.Response.Write("<br/>");
        HttpContext.Current.Response.Write(ConfigurationManager.AppSettings["softwarename"].ToString());
        HttpContext.Current.Response.End();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
    }


    protected void gvCDISIREMatrix_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (Request.QueryString["categoryid"] != null)
        {
            BindData();
        }
    }

    private void BindData()
    {
        try
        {
            //int iRowCount = 0;
            //int iTotalPageCount = 0;

            DataTable dt = new DataTable();
            dt = PhoenixInspectionCDISIREMatrix.EditCDISIREMatrixColumnHeaders(General.GetNullableGuid(ViewState["CATEGORYID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);


            DataTable dtcolumn = new DataTable();

            dtcolumn = PhoenixInspectionCDISIREMatrix.CDISIREMatrixSummaryVerificationView(General.GetNullableGuid(ViewState["CATEGORYID"].ToString()), General.GetNullableGuid(null), PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                  , General.GetNullableInteger(ViewState["COUNTTYPE"].ToString()));

            if (dtcolumn.Rows.Count > 0)
            {
                ShowQuery(General.GetNullableGuid(ViewState["CATEGORYID"].ToString()), dt, dtcolumn);
            }
            else
            {
                gvCDISIREMatrix.Visible = false;
                //dtcolumn.Rows.Clear();
                //DataTable dt1 = dtcolumn;
                //ShowNoRecordsFound(dt1, gvCDISIREMatrix);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCDISIREMatrix_RowCommand(object sender, GridCommandEventArgs e)
    {

        GridEditableItem eeditedItem = e.Item as GridEditableItem;
        RadGrid _gridView = (RadGrid)sender;

        int nCurrentRow = e.Item.ItemIndex;

        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            GridDataItem item = (GridDataItem)e.Item;

            //string lblid = ((RadLabel)_gridView.Items[nCurrentRow].FindControl("lblid")).Text;

            Guid lblid = new Guid(item.GetDataKeyValue("FLDCONTENTID").ToString());

            //string lblprocid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblprocid")).Text;
            //string lblcategoryid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblcategoryid")).Text;
            //if (lblStatusid == PhoenixCommonRegisters.GetHardCode(1, 168, "S4")
            //    || lblStatusid == PhoenixCommonRegisters.GetHardCode(1, 168, "S5"))
            //    Filter.CurrentSelectedIncidentMenu = "ilog";
            //else
            //    Filter.CurrentSelectedIncidentMenu = null;

            Response.Redirect("../Inspection/InspectionCDISIREMatrixContentAdd.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&noofcolumns=" + General.GetNullableInteger(ViewState["NOOFCOLUMNS"].ToString())
                                    + "&contentid=" + lblid + "&action=edit");
        }

        if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            Guid lblid = new Guid(item.GetDataKeyValue("FLDCONTENTID").ToString());
            PhoenixInspectionCDISIREMatrix.DeleteCDISIREMatrixContent(lblid);
            ucStatus.Text = "Document is deleted.";
            BindData();
        }

        if (e.CommandName.ToUpper().Equals("ATTACHMENT"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            Guid lblid = new Guid(item.GetDataKeyValue("FLDCONTENTID").ToString());
            BindData();
        }

        if (e.CommandName.ToUpper().Equals("BPGATTACHMENT"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            Guid lblid = new Guid(item.GetDataKeyValue("FLDCONTENTID").ToString());
            BindData();
        }

        if (e.CommandName.ToUpper().Equals("COMMENTS"))
        {
            GridDataItem item = (GridDataItem)e.Item;

            Guid lblid = new Guid(item.GetDataKeyValue("FLDCONTENTID").ToString());
        }

        if (e.CommandName.ToUpper().Equals("ADD"))
        {
            //BindData();
            // GridFooterItem item = (GridFooterItem)e.Item;
            GridFooterItem footerItem = gvCDISIREMatrix.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
            RadTextBox txt = (RadTextBox)footerItem.FindControl("txtbox1");

            if (ViewState["CATEGORYID"] != null)
            {
                int n = int.Parse(ViewState["NOOFCOLUMNS"].ToString());
                if (n > 0)
                {
                    // string column = "";
                    string columnvalue = "";
                    for (int i = 1; i <= n; i++)
                    {
                        RadTextBox MyTextBox = new RadTextBox();
                        if (MyTextBox != null)
                        {
                            string columnheader = "";
                            string ss = ((RadTextBox)footerItem.FindControl("txtbox1")).Text;
                            columnheader = "COLUMN" + i.ToString();
                            DataTable ds = new DataTable();
                            ds = PhoenixInspectionCDISIREMatrix.EditCDISIREMatrixColumnHeaders(General.GetNullableGuid(ViewState["CATEGORYID"].ToString()));
                            //column = ds.Rows[0][columnheader].ToString();
                            columnvalue = ss;
                            if (MyTextBox != null)
                            {
                                PhoenixInspectionCDISIREMatrix.UpdateCDISIREMatrixCategoryContent(
                                                                     General.GetNullableGuid(ViewState["FLDCONTENTID"].ToString()),
                                                                     General.GetNullableGuid(ViewState["CATEGORYID"].ToString()),
                                                                     null,
                                                                     null,
                                                                     General.GetNullableString(columnheader),
                                                                     General.GetNullableString(columnvalue)
                                                                     );
                            }

                        }
                    }
                }
            }

        }

    }

    protected void gvCDISIREMatrix_ItemDataBound(Object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {

            GridDataItem item = (GridDataItem)e.Item;
            HtmlTable tbl = (HtmlTable)e.Item.FindControl("tblForms");
            HtmlTable tblReports = (HtmlTable)e.Item.FindControl("tblReports");
            //RadioButtonList rbl = (RadioButtonList)e.Row.FindControl("rblType");   
            //Guid lblid = new Guid(item.GetDataKeyValue("FLDID").ToString());
            RadLabel lblid = (RadLabel)e.Item.FindControl("lblid");
            if (lblid != null)
            {
                ViewState["CONTENTID"] = lblid.Text;
            }
            else
            {
                ViewState["CONTENTID"] = null;
            }

            LinkButton del = (LinkButton)item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton edit = (LinkButton)item.FindControl("Edit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            if (del != null)
            {
                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            LinkButton cmd = (LinkButton)e.Item.FindControl("cmdComments");
            if (cmd != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmd.CommandName))
                    cmd.Visible = false;

                cmd.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionCDISIREMatrixComments.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&contentid=" + General.GetNullableGuid(ViewState["CONTENTID"].ToString()) + "&inspectionid=" + General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()) + "'); return false;");
            }

            LinkButton cmdClient = (LinkButton)e.Item.FindControl("cmdClientComments");
            if (cmdClient != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdClient.CommandName))
                    cmdClient.Visible = false;

                cmdClient.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionCDISIREClientBPGComments.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&contentid=" + General.GetNullableGuid(ViewState["CONTENTID"].ToString()) + "&inspectionid=" + General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()) + "'); return false;");
            }

            LinkButton cmdOnboardChecks = (LinkButton)e.Item.FindControl("cmdOnboardChecks");
            RadCheckBox chkSelect = (RadCheckBox)e.Item.FindControl("chkSelect");


            if (cmdOnboardChecks != null && PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdOnboardChecks.CommandName))
                    cmdOnboardChecks.Visible = false;

                cmdOnboardChecks.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionCDISIREMatrixOnboardChecks.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&contentid=" + General.GetNullableGuid(ViewState["CONTENTID"].ToString()) + "&inspectionid=" + General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()) + "'); return false;");

                edit.Visible = false;
                del.Visible = false;
                chkSelect.Enabled = false;

            }
            else
            {
                cmdOnboardChecks.Visible = false;
            }

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            LinkButton BPGAtt = (LinkButton)e.Item.FindControl("BPGAtt");
            RadLabel lblDTkey = (RadLabel)e.Item.FindControl("lblDTkey");
            RadLabel lblVesselid = (RadLabel)e.Item.FindControl("lblVesselid");
            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (att != null)
            {
                att.Visible = false;
                //att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                //HtmlGenericControl html = new HtmlGenericControl();
                //if (drv["FLDISATTACHMENT"].ToString() == "0")
                //{
                //    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip\"></i></span>";
                //    att.Controls.Add(html);
                //}
                //else
                //{
                //    html.InnerHtml = "<span class=\"icon\" style=\"color:skyblue\"><i class=\"fas fa-paperclip\"></i></span>";
                //    att.Controls.Add(html);
                //}
                ////    att.ImageUrl = Session["images"] + "/no-attachment.png";
                //att.Attributes.Add("onclick", "javascript:openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                //    + PhoenixModule.QUALITY + "&type=SHIPBOARDEVIDENCE&cmdname=SHIPBOARDEVIDENCEUPLOAD&VESSELID=" + lblVesselid.Text + "'); return true;");
            }

            //if (att != null && drv["FLDVESSELID"].ToString() != "0")
            //{
            //    att.Enabled = false;
            //}

            if (BPGAtt != null)
            {
                BPGAtt.Visible = SessionUtil.CanAccess(this.ViewState, BPGAtt.CommandName);
                HtmlGenericControl html = new HtmlGenericControl();
                if (drv["FLDISBPGATTACHMENT"].ToString() == "0")
                {
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip\"></i></span>";
                    BPGAtt.Controls.Add(html);
                }
                else
                {
                    html.InnerHtml = "<span class=\"icon\" style=\"color:skyblue\"><i class=\"fas fa-paperclip\"></i></span>";
                    BPGAtt.Controls.Add(html);
                }
                //    att.ImageUrl = Session["images"] + "/no-attachment.png";
                BPGAtt.Attributes.Add("onclick", "javascript:openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.QUALITY + "&type=BPGEXAMPLE&cmdname=SHIPBOARDEVIDENCEUPLOAD&VESSELID=" + lblVesselid.Text + "'); return true;");
            }


            if (lblid != null)
            {

                DataSet dss = PhoenixInspectionCDISIREMatrix.FormPosterList(ViewState["CONTENTID"] == null ? null : General.GetNullableGuid(ViewState["CONTENTID"].ToString()),
                                                        General.GetNullableGuid(ViewState["CATEGORYID"].ToString()), 0,PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                int number = 1;
                if (dss.Tables[0].Rows.Count > 0 && dss.Tables[0].Columns.Count > 1)
                {
                    tbl.Rows.Clear();
                    foreach (DataRow dr in dss.Tables[0].Rows)
                    {

                        HyperLink hl = new HyperLink();
                        hl.Text = dr["FLDNAME"].ToString();
                        hl.ID = "hlink" + number.ToString();
                        hl.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");

                        int type = 0;
                        PhoenixIntegrationQuality.GetSelectedeTreeNodeType(General.GetNullableGuid(dr["FLDFORMPOSTERID"].ToString()), ref type);
                        if (type == 2)
                            hl.Attributes.Add("onclick", "openNewWindow('codehelp1', '', 'DocumentManagement/DocumentManagementDocumentGeneral.aspx?showall=0&DOCUMENTID=" + dr["FLDFORMPOSTERID"].ToString() + "');return false;");
                        else if (type == 3)
                            hl.Attributes.Add("onclick", "openNewWindow('codehelp1', '', 'DocumentManagement/DocumentManagementDocumentSectionContentView.aspx?callfrom=documenttree&SECTIONID=" + dr["FLDFORMPOSTERID"].ToString() + "');return false;");
                        else if (type == 5)
                        {
                            DataSet ds = PhoenixIntegrationQuality.FormEdit(
                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                    , new Guid(hl.ID.ToString()));

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                DataRow drr = ds.Tables[0].Rows[0];
                                hl.Attributes.Add("onclick", "openNewWindow('codehelp1', '', 'StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&FORMID=" + dr["FLDFORMPOSTERID"].ToString() + "&FORMREVISIONID=" + drr["FLDFORMREVISIONID"].ToString() + "');return false;");
                            }
                        }
                        else if (type == 6)
                        {
                            DataSet ds = PhoenixIntegrationQuality.FormEdit(
                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                    , new Guid(dr["FLDFORMPOSTERID"].ToString()));

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                DataRow drr = ds.Tables[0].Rows[0];
                                if (drr["FLDFORMREVISIONDTKEY"] != null && General.GetNullableGuid(drr["FLDFORMREVISIONDTKEY"].ToString()) != null)
                                {
                                    DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(drr["FLDFORMREVISIONDTKEY"].ToString()));
                                    if (dt.Rows.Count > 0)
                                    {
                                        DataRow drRow = dt.Rows[0];
                                        //ifMoreInfo.Attributes["src"] = Session["sitepath"] + "/attachments/" + drRow["FLDFILEPATH"].ToString();                                              
                                        hl.Target = "_blank";
                                        hl.NavigateUrl = "../Common/download.aspx?dtkey=" + drRow["FLDDTKEY"].ToString();                                        
                                    }
                                }
                            }
                        }

                        HtmlTableRow tr = new HtmlTableRow();
                        HtmlTableCell tc = new HtmlTableCell();
                        //tc.Controls.Add(cb);
                        //tr.Cells.Add(tc);
                        tc = new HtmlTableCell();
                        tc.Controls.Add(hl);
                        tr.Cells.Add(tc);
                        tbl.Rows.Add(tr);
                        tc = new HtmlTableCell();
                        tc.InnerHtml = "<br/>";
                        tr.Cells.Add(tc);
                        tbl.Rows.Add(tr);
                        number = number + 1;
                    }
                    tbl.Visible = true;
                }
                DataSet dsreports = PhoenixInspectionCDISIREMatrix.FormReportList(ViewState["CONTENTID"] == null ? null : General.GetNullableGuid(ViewState["CONTENTID"].ToString()),
                                                        General.GetNullableGuid(ViewState["CATEGORYID"].ToString()));
                int no = 1;
                if (dsreports.Tables[0].Rows.Count > 0 && dsreports.Tables[0].Columns.Count > 1)
                {
                    tblReports.Rows.Clear();
                    foreach (DataRow dr in dsreports.Tables[0].Rows)
                    {
                        HyperLink hlReports = new HyperLink();
                        hlReports.Text = dr["FLDNAME"].ToString();
                        hlReports.ID = "hlReportslink" + no.ToString();
                        hlReports.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");
                        hlReports.Attributes.Add("onclick", "openNewWindow('codehelp1', '', 'StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&ReportId=" + dr["FLDFORMREPORTID"].ToString() + "');return false;");
                        HtmlTableRow tr = new HtmlTableRow();
                        HtmlTableCell tc = new HtmlTableCell();
                        //tc.Controls.Add(cb);
                        //tr.Cells.Add(tc);
                        tc = new HtmlTableCell();
                        tc.Controls.Add(hlReports);
                        tr.Cells.Add(tc);
                        tblReports.Rows.Add(tr);
                        tc = new HtmlTableCell();
                        tc.InnerHtml = "<br/>";
                        tr.Cells.Add(tc);
                        tblReports.Rows.Add(tr);
                        no = no + 1;
                    }
                    tblReports.Visible = true;
                }

            }
        }

        if (e.Item is GridFooterItem)
        {
            GridFooterItem item = (GridFooterItem)e.Item;
            RadCheckBoxList chkDeptList = ((RadCheckBoxList)e.Item.FindControl("chkDeptList"));

            LinkButton db = (LinkButton)item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

            if (chkDeptList != null)
            {
                DataSet ds = PhoenixRegistersDepartment.Listdepartment(1, null);
                DataTable dt = ds.Tables[0];
                chkDeptList.DataSource = dt;
                chkDeptList.DataBindings.DataTextField = "FLDDEPARTMENTNAME";
                chkDeptList.DataBindings.DataValueField = "FLDDEPARTMENTID";
                chkDeptList.DataBind();
            }

        }

    }



    private void ShowQuery(Guid? categoryid, DataTable dt, DataTable dtcolumn)
    {

        int columncount = AddTemplateColumn(categoryid, dt, dtcolumn);
        int noofcolumns = int.Parse(dt.Rows[0]["FLDNOOFCOLUMNS"].ToString());

        if (columncount > 0)
        {
            if (dtcolumn.Rows.Count > 0)
            {
                gvCDISIREMatrix.DataSource = dt;
                gvCDISIREMatrix.DataSource = dtcolumn;
            }
            else
            {
                gvCDISIREMatrix.DataSource = dt;
            }
            for (int i = noofcolumns + 8; i < gvCDISIREMatrix.Columns.Count; i++)
            {
                gvCDISIREMatrix.Columns[i].Visible = false;
            }
            gvCDISIREMatrix.DataBind();

        }
    }

    private int AddTemplateColumn(Guid? queryid, DataTable dt, DataTable dtcolumn)
    {

        int noofcolumns = int.Parse(dt.Rows[0]["FLDNOOFCOLUMNS"].ToString());
        string columnheader = "";
        string columnheadervalue = "";
        string columnheadertext = "";
        //string column = "";

        if (dtcolumn.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < noofcolumns; i++)
                {
                    int j = i + 1;
                    GridTemplateColumn field = new GridTemplateColumn();
                    columnheader = "FLDCOLUMN" + (j).ToString();
                    columnheadertext = "FLDCOLUMNTEXT" + (j).ToString();
                    columnheadervalue = columnheader;
                    field.HeaderTemplate = new GridViewTemplate(ListItemType.Header, dr[columnheadertext].ToString());
                    field.ItemTemplate = new GridViewTemplate(ListItemType.Item, columnheadervalue, "Label");
                    //field.FooterTemplate = new GridViewTemplate(ListItemType.Footer);
                    //gvCDISIREMatrix.Columns.Add(field); 
                    gvCDISIREMatrix.Columns.AddAt(i, field);
                }
            }
        }
        else
        {
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 1; i <= noofcolumns; i++)
                {
                    int j = i + 1;
                    GridTemplateColumn field = new GridTemplateColumn();
                    columnheader = "FLDCOLUMN" + (j).ToString();
                    columnheadertext = "FLDCOLUMNTEXT" + (j).ToString();
                    columnheadervalue = columnheader;
                    field.HeaderTemplate = new GridViewTemplate(ListItemType.Header, dr[columnheadertext].ToString());
                    //field.ItemTemplate = new GridViewTemplate(ListItemType.Item, columnheadervalue, "Label");
                    //field.FooterTemplate = new GridViewTemplate(ListItemType.Footer, dr["FLDADD"].ToString(), "LinkButton");
                    gvCDISIREMatrix.Columns.Add(field);
                }

            }
        }

        return dt.Rows.Count;
    }


    public class GridViewTemplate : ITemplate
    {
        ListItemType _templatetype;
        string _columnname;
        string _fieldtype;
        int _textboxid;


        public GridViewTemplate(ListItemType type, string columnname)
        {
            _templatetype = type;
            _columnname = columnname;
            _fieldtype = "Label";
        }

        public GridViewTemplate(ListItemType type, int i, string columnname, string fieldtype)
        {
            _templatetype = type;
            _columnname = columnname;
            _fieldtype = "RadTextBox";
            _textboxid = i;

        }

        public GridViewTemplate(ListItemType type, string columnname, string fieldtype)
        {
            _templatetype = type;
            _columnname = columnname;
            _fieldtype = fieldtype;
        }

        //protected void gvCDISIREMatrix_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        // CREATE A LinkButton AND IT TO EACH ROW.
        //        LinkButton lb = new LinkButton();
        //        lb.ID = "lbBooks";
        //        lb.Text = "Select";
        //        e.Row.Cells[3].Controls.Add(lb);
        //    }
        //}


        private void OnDataBinding(object sender, EventArgs e)
        {
            Control ctl = (Control)sender;

            GridDataItem dataitemcontainer = (GridDataItem)ctl.NamingContainer;
            object boundvalue = DataBinder.Eval(dataitemcontainer.DataItem, _columnname);

            switch (_templatetype)
            {
                case ListItemType.Item:
                    PopulateField(sender, _fieldtype, boundvalue);
                    break;

                case ListItemType.Header:
                    PopulateField(sender, _fieldtype, boundvalue);
                    break;

                case ListItemType.Footer:
                    PopulateField(sender, _fieldtype, boundvalue);
                    break;
            }
        }

        private void PopulateField(object sender, string fieldtype, object value)
        {
            switch (fieldtype.ToUpper())
            {
                case "LABEL":
                    Label lbl = (Label)sender;
                    lbl.Text = value.ToString();
                    break;
                case "LINKBUTTON":
                    LinkButton lnk = (LinkButton)sender;
                    lnk.Text = "add";
                    lnk.ID = "cmdEdit";
                    lnk.Attributes.Add("runat", "server");
                    break;
                case "RADTEXTBOX":
                    RadTextBox txt = (RadTextBox)sender;
                    txt.ID = "txtbox" + _textboxid;
                    //txt.Text = value.ToString();
                    break;
            }
        }

        void ITemplate.InstantiateIn(System.Web.UI.Control container)
        {
            switch (_templatetype)
            {
                case ListItemType.Header:
                    Literal ltl = new Literal();
                    ltl.Text = _columnname;
                    container.Controls.Add(ltl);
                    break;

                case ListItemType.Item:
                    switch (_fieldtype.ToUpper())
                    {
                        case "LABEL":
                            Label lbl = new Label();
                            lbl.DataBinding += new EventHandler(OnDataBinding);
                            container.Controls.Add(lbl);
                            break;
                        case "LINKBUTTON":
                            LinkButton lnk = new LinkButton();
                            lnk.DataBinding += new EventHandler(OnDataBinding);
                            container.Controls.Add(lnk);
                            break;
                        default:
                            ltl = new Literal();
                            ltl.DataBinding += new EventHandler(OnDataBinding);
                            container.Controls.Add(ltl);
                            break;
                    }
                    break;

                case ListItemType.EditItem:
                    break;

                case ListItemType.Footer:
                    RadTextBox ltl1 = new RadTextBox();
                    ltl1.ID = "txtbox" + _textboxid;
                    ltl1.Attributes.Add("runat", "server");
                    //ltl1.ClientIDMode = ClientIDMode.AutoID;                    
                    //ltl1.Text = _columnname;
                    // ltl1.DataBinding += new EventHandler(OnDataBinding);
                    container.Controls.Add(ltl1);
                    //_textboxid++;
                    break;

            }
        }
    }

    protected void txtnoofcolumns_changed(object sender, EventArgs e)
    {
        //CreateDynamicTextBox();
    }

    protected void btn_Click(object sender, EventArgs e)
    {


    }
    protected void MenuDocumentCategoryMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;

        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("NEW"))
            {
             
            }

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


    protected void EvidenceRequired(Object sender, EventArgs e)
    {
        ArrayList SelectedSections = new ArrayList();
        Guid index = new Guid();

        foreach (GridDataItem item in gvCDISIREMatrix.Items)
        {
            if (item.GetDataKeyValue("FLDCONTENTID").ToString() != "")
            {
                index = new Guid(item.GetDataKeyValue("FLDCONTENTID").ToString());

                PhoenixInspectionCDISIREMatrix.EvidenceRequiredUpdate(General.GetNullableGuid(index.ToString()),
                                                (((RadCheckBox)item.FindControl("chkSelect")).Checked == true) ? 1 : 0, PhoenixSecurityContext.CurrentSecurityContext.VesselID);

            }
        }
        if (Request.QueryString["categoryid"] != null || ViewState["CONTENTID"] != null)
        {
            BindData();
        }
    }
}