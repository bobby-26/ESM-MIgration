using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Integration;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
using System.Configuration;
using SouthNests.Phoenix.Registers;
using System.Collections;

public partial class Inspection_InspectionTMSAMatrixCategoryContentList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        lblSelectedNode.Visible = false;
        lblCategoryId.Visible = false;
        configpart.Visible = false;

        PhoenixToolbar toolbarmain = new PhoenixToolbar();

        if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode == "OFFSHORE")
        {
            MenuDocumentCategoryMain.Title = "OVMSA Matrix";
        }
        else
            MenuDocumentCategoryMain.Title = "TMSA Matrix";

        MenuDocumentCategoryMain.AccessRights = this.ViewState;
        MenuDocumentCategoryMain.MenuList = toolbarmain.Show();


        if (!IsPostBack)
        {
            confirmTMSAReview.Attributes.Add("style", "display:none;");
            confirmTMSAUnlock.Attributes.Add("style", "display:none;");

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["CATEGORYID"] = "";
            ViewState["INSPECTIONID"] = "";
            ViewState["NOOFCOLUMNS"] = "";
            ViewState["NOOFSTAGES"] = "";
            ViewState["ERROR"] = "";
            ViewState["LOCK"] = "";


            if (Request.QueryString["categoryid"] != null)
                lblSelectedNode.Text = Request.QueryString["categoryid"].ToString();

            if (Request.QueryString["noofcolumns"] != null)
                txtnoofcolumns.Text = Request.QueryString["noofcolumns"].ToString();

            DataSet ds = PhoenixRegistersDepartment.Listdepartment(1, 2, null);
            DataTable dt = ds.Tables[0];
            ddldeptlist.DataSource = dt;
            ddldeptlist.DataBind();
            ddldeptlist.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

            ucCompany.Enabled = false;
            ucCompany.SelectedCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString();
            BindDocumentCategoryTree();
            DocumentCategoryEdit();
            CreateDynamicTextBox();
            MenuPhoenixQuery.Visible = false;
            BindData();
            MenuPhoenixQuery.Visible = true;
            BindCompany();
        }
        setMenu();
        if ((Request.QueryString["categoryid"] != null || ViewState["CONTENTID"] != null) && ViewState["ERROR"].ToString() == "1")
        {
            BindData();
            ViewState["ERROR"] = "";
        }

    }

    protected void BindCompany()
    {
        DataSet ds = PhoenixInspectionOilMajorComany.ListOilMajorCompany(1, null);

        ddlCompany.DataSource = ds.Tables[0];
        ddlCompany.DataTextField = "FLDOILMAJORCOMPANYNAME";
        ddlCompany.DataValueField = "FLDOILMAJORCOMPANYID";
        ddlCompany.DataBind();
        ddlCompany.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    private void setMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionTMSAMatrixCategoryContentList.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        //toolbar.AddImageLink("javascript:parent.Openpopup('Filter','','../Inspection/InspectionTMSAMatrixContentAdd.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&noofcolumns=" + General.GetNullableInteger(ViewState["NOOFCOLUMNS"].ToString()) + "'); return false;", "Add", "add.png", "ADD");
        //toolbar.AddImageLink("javascript:Openpopup('codehelp1','','InspectionIncidentAdd.aspx');return true;", "Add New", "add.png", "ADD");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionTMSAMatrixCategoryContentList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
        if (ViewState["LOCK"].ToString() == "0")
        {
            toolbar.AddImageButton("../Inspection/InspectionTMSAMatrixCategoryContentList.aspx", "Publish", "approve.png", "PUBLISH");
        }
        toolbar.AddImageButton("../Inspection/InspectionTMSAMatrixCategoryContentList.aspx", "Revisions", "requisition.png", "REVISIONS");
        toolbar.AddImageLink("javascript:parent.openNewWindow('Filter','','Inspection/InspectionTMSAVerificationSummary.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&inspectionid=" + General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()) + "'); return false;", "Verification Summary", "checklist.png", "SUMMARY");
        toolbar.AddImageLink("javascript:parent.openNewWindow('Filter','','Inspection/InspectionTMSADepartmentWiseSummary.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&inspectionid=" + General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()) + "'); return false;", "Departmentwise Summary", "showall.png", "DEPTSUMMARY");
        toolbar.AddImageLink("javascript:parent.openNewWindow('Filter','','Inspection/InspectionTMSAMatrixCategoryContentArchivedList.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&inspectionid=" + General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()) + "'); return false;", "Archived List", "Archive.png", "ARCHIVE");
        if (ViewState["LOCK"].ToString() == "1")
        {
            toolbar.AddImageButton("../Inspection/InspectionTMSAMatrixCategoryContentList.aspx", "Unlock", "unlock.png", "UNLOCK");
        }
        toolbar.AddImageButton("../Inspection/InspectionTMSAMatrixCategoryContentList.aspx", "Refresh", "refresh.png", "REFRESH", ToolBarDirection.Right);
        //toolbar.AddFontAwesomeButton("../Inspection/InspectionTMSAMatrixCategoryContentList.aspx", "Comments", "<i class=\"fa fa-comments\"></i>", "COMMENTS");
        //toolbar.AddFontAwesomeButton("javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionTMSAMatrixComments.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "'); return false;", "Comments", "<i class=\"fa fa-comments\"></i>", "COMMENTS");
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

            if (ViewState["CATEGORYID"].ToString() != "" && ViewState["INSPECTIONID"] != null)
            {
                DataTable dt = new DataTable();
                dt = PhoenixInspectionTMSAMatrix.EditTMSAMatrixColumnHeaders(General.GetNullableGuid(lblSelectedNode.Text));
                if (dt.Rows[0]["FLDCOLUMN2"].ToString() != "" || dt.Rows[0]["FLDCOLUMN3"].ToString() != "")
                {

                    Response.Redirect("../Inspection/InspectionTMSAMatrixContentAdd.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&noofcolumns=" + General.GetNullableInteger(ViewState["NOOFCOLUMNS"].ToString()) + "&noofstages=" + General.GetNullableInteger(ViewState["NOOFSTAGES"].ToString())
                                        + "&inspectionid=" + General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()) + "&action=add");
                }
            }
            else
            {
                ucError.ErrorMessage = "Select the category";
                ucError.Visible = true;
            }

        }

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }

        if (CommandName.ToUpper().Equals("PUBLISH"))
        {
            if (ViewState["CATEGORYID"].ToString() != "" && ViewState["CATEGORYID"] != null)
            {
                RadWindowManager3.RadConfirm("Are you sure you want to publish the document.? Y/N", "confirmTMSAReview", 320, 150, null, "Confirm");
                if (lblSelectedNode.Text != "")
                {
                    BindData();
                }
            }
            else
            {
                ucError.ErrorMessage = "Select the category";
                ucError.Visible = true;
            }
        }

        if (CommandName.ToUpper().Equals("UNLOCK"))
        {
            if (ViewState["CATEGORYID"].ToString() != "" && ViewState["CATEGORYID"] != null)
            {
                RadWindowManager3.RadConfirm("Are you sure you want to unlock the document.? Y/N", "confirmTMSAUnlock", 320, 150, null, "ConfirmUnlock");
                if (lblSelectedNode.Text != "")
                {
                    BindData();
                }
            }
            else
            {
                ucError.ErrorMessage = "Select the category";
                ucError.Visible = true;
            }
        }

        if (CommandName.ToUpper().Equals("REVISIONS"))
        {
            Response.Redirect("../Inspection/InspectionTMSAMatrixCategoryRevisions.aspx?CATEGORYID=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()));
        }

        if (CommandName.ToUpper().Equals("REFRESH"))
        {
            BindData();
        }

    }

    protected void ShowExcel()
    {

        DataTable dt = new DataTable();
        dt = PhoenixInspectionTMSAMatrix.EditTMSAMatrixColumnHeaders(General.GetNullableGuid(lblSelectedNode.Text));

        string captions = "";
        string columns = "";
        string columnname = "FLDCOLUMN";


        if (dt.Rows.Count > 0)
        {
            int noofcolumns = int.Parse(dt.Rows[0]["FLDNOOFCOLUMNS"].ToString());
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

        columns = columns + ",FLDPROCEDURES,FLDOBJECTIVEEVIDENCE,FLDDEPARTMENTNAMELIST,FLDCOMPLY,FLDISOFFICEACCEPTED";
        captions = captions + ",Procedures, Objective Evidence,Responsibility,Comply,Accepted";

        string[] alColumns = columns.Split(',');
        string[] alCaptions = captions.Split(',');


        DataTable dtcolumn = new DataTable();

        dtcolumn = PhoenixInspectionTMSAMatrix.TMSAMatrixCategoryContentSearch(General.GetNullableGuid(lblSelectedNode.Text), General.GetNullableGuid(ddlCompany.SelectedValue));

        if (dtcolumn.Rows.Count > 0)
        {
            ShowQuery(General.GetNullableGuid(lblSelectedNode.Text), dt, dtcolumn);
        }
        else
        {
            dtcolumn.Rows.Clear();
            gvTMSAMatrix.DataSource = dtcolumn;
            //dtcolumn.Rows.Clear();
            //DataTable dt1 = dtcolumn;
            //ShowNoRecordsFound(dt1, gvTMSAMatrix);
        }
        ShowExcel("TMSA Matrix", dtcolumn, alColumns, alCaptions, null, null);
    }
    public static void ShowExcel(string strHeading, DataTable dt, string[] alColumns, string[] alCaptions, int? strSortDirection, string strSortExpression)
    {

        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + (string.IsNullOrEmpty(strHeading) ? "Attachment" : strHeading.Replace(" ", "_")) + ".xls");
        HttpContext.Current.Response.ContentType = "application/vnd.msexcel";
        HttpContext.Current.Response.Write("<style>.text{ mso-number-format:\"\\@\";}</style>");
        HttpContext.Current.Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");

        HttpContext.Current.Response.Write("<tr>");
        HttpContext.Current.Response.Write("<td ><img src='http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        if (dt.Rows.Count > 0)
        {
            HttpContext.Current.Response.Write("<td colspan='" + (alColumns.Length - 1).ToString() + "'><h3>&nbsp&nbsp&nbsp&nbsp " + strHeading + "-" + dt.Rows[0]["FLDCATEGORYNAME"].ToString() + "</h3></td>");
        }
        else
            HttpContext.Current.Response.Write("<td colspan='" + (alColumns.Length - 1).ToString() + "'><h3>&nbsp&nbsp&nbsp&nbsp " + strHeading + "</h3></td>");
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

    private void BindDocumentCategoryTree()
    {

        DataSet ds = new DataSet();

        int companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

        ds = PhoenixInspectionTMSAMatrix.ListTreeTMSAMatrixCategory(companyid);

        tvwDocumentCategory.DataTextField = "FLDCATEGORYNAME";
        tvwDocumentCategory.DataValueField = "FLDCATEGORYID";
        tvwDocumentCategory.DataFieldParentID = "FLDPARENTGROUPID";
        tvwDocumentCategory.RootText = "ROOT";

        if (ds.Tables[0].Rows.Count > 0)
        {
            tvwDocumentCategory.PopulateTree(ds.Tables[0]);
            tvwDocumentCategory.SelectNodeByValue = lblSelectedNode.Text;
        }
        else
        {
            tvwDocumentCategory.RootText = "";
            tvwDocumentCategory.PopulateTree(ds.Tables[0]);
        }
        //CreateDynamicTextBox();
    }

    protected void ucTree_SelectNodeEvent(object sender, EventArgs e)
    {
        RadTreeNodeEventArgs tvsne = (RadTreeNodeEventArgs)e;
        lblSelectedNode.Visible = false;
        lblSelectedNode.Text = tvsne.Node.Value.ToString();
        lblCategoryId.Text = tvsne.Node.Text.ToString();
        // ViewState["CATEGORYID"] = lblSelectedNode.Text;

        if (lblSelectedNode.Text == "_Root")
        {

            lblSelectedNode.Text = "";
            lblCategoryId.Text = "";
            txtCategoryNumber.Text = "";
            txtDocumentCategory.Text = "";
            txtnoofcolumns.Text = "";
            MenuPhoenixQuery.Visible = false;
            gvTMSAMatrix.Visible = false;
        }
        else
        {
            gvTMSAMatrix.Visible = true;
            lblSelectedNode.Text = tvsne.Node.Value.ToString();
            lblCategoryId.Text = tvsne.Node.Text.ToString();

            ddlCompany.SelectedIndex = 0;
            DocumentCategoryEdit();
            CreateDynamicTextBox();
            BindData();
            configpart.Visible = false;
            DataSet ds = new DataSet();
            if (!string.IsNullOrEmpty(lblSelectedNode.Text))
            {
                ds = PhoenixInspectionTMSAMatrix.TMSAMatrixCategoryEdit(new Guid(lblSelectedNode.Text));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["INSPECTIONID"] = ds.Tables[0].Rows[0]["FLDINSPECTIONID"].ToString();
                    if (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["FLDPARENTCATEGORYID"].ToString()))
                    {
                        txtnoofcolumns.Enabled = true;
                    }
                    else
                    {
                        txtnoofcolumns.Enabled = false;
                    }
                }
            }

        }
        setMenu();
    }


    protected void gvTMSAMatrix_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        //if (Request.QueryString["categoryid"] != null)
        //{
        BindData();
        //}
    }

    private void BindData()
    {
        try
        {
            //int iRowCount = 0;
            //int iTotalPageCount = 0;

            DataTable dt = new DataTable();
            dt = PhoenixInspectionTMSAMatrix.EditTMSAMatrixColumnHeaders(General.GetNullableGuid(lblSelectedNode.Text), null);

            DataTable dtcolumn = new DataTable();

            dtcolumn = PhoenixInspectionTMSAMatrix.TMSAMatrixCategoryContentSearch(General.GetNullableGuid(lblSelectedNode.Text),
                                                                                    General.GetNullableGuid(ddlCompany.SelectedValue),
                                                                                    General.GetNullableInteger(ddldeptlist.SelectedValue));

            if (dtcolumn.Rows.Count > 0)
            {
                ShowQuery(General.GetNullableGuid(lblSelectedNode.Text), dt, dtcolumn);
            }
            else
            {
                gvTMSAMatrix.DataSource = dtcolumn;
                gvTMSAMatrix.DataBind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTMSAMatrix_RowCommand(object sender, GridCommandEventArgs e)
    {

        GridEditableItem eeditedItem = e.Item as GridEditableItem;
        RadGrid _gridView = (RadGrid)sender;

        int nCurrentRow = e.Item.ItemIndex;

        try
        {
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                GridDataItem item = (GridDataItem)e.Item;

                //string lblid = ((RadLabel)_gridView.Items[nCurrentRow].FindControl("lblid")).Text;

                Guid lblid = new Guid(item.GetDataKeyValue("FLDCONTENTID").ToString());

                Response.Redirect("../Inspection/InspectionTMSAMatrixContentAdd.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&noofcolumns=" + General.GetNullableInteger(ViewState["NOOFCOLUMNS"].ToString())
                                        + "&contentid=" + lblid + "&inspectionid=" + General.GetNullableInteger(ViewState["INSPECTIONID"].ToString()) + "&action=edit");
            }

            if (e.CommandName.ToUpper().Equals("CHANGEPAGESIZE"))
                return;

            if (e.CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                Guid lblid = new Guid(item.GetDataKeyValue("FLDCONTENTID").ToString());
                BindData();
            }

            if (e.CommandName.ToUpper().Equals("OFFICECHECKS"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                BindData();
            }

            if (e.CommandName.ToUpper().Equals("BPGATTACHMENT"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                Guid lblid = new Guid(item.GetDataKeyValue("FLDCONTENTID").ToString());
                BindData();
            }

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                Guid lblid = new Guid(item.GetDataKeyValue("FLDCONTENTID").ToString());
                PhoenixInspectionTMSAMatrix.DeleteTMSAMatrixContent(lblid);
                ucStatus.Text = "Document is deleted.";
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
                GridFooterItem footerItem = gvTMSAMatrix.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
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
                                ds = PhoenixInspectionTMSAMatrix.EditTMSAMatrixColumnHeaders(General.GetNullableGuid(ViewState["CATEGORYID"].ToString()));
                                //column = ds.Rows[0][columnheader].ToString();
                                columnvalue = ss;
                                if (MyTextBox != null)
                                {
                                    PhoenixInspectionTMSAMatrix.UpdateTMSAMatrixCategoryContent(
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            BindData();
            ViewState["ERROR"] = "1";
        }


    }

    protected void gvTMSAMatrix_ItemDataBound(Object sender, Telerik.Web.UI.GridItemEventArgs e)
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
            LinkButton cmdedit = (LinkButton)e.Item.FindControl("Edit");

            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            if (cmdedit != null) cmdedit.Visible = SessionUtil.CanAccess(this.ViewState, cmdedit.CommandName);

            if (del != null)
            {
                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            LinkButton cmd = (LinkButton)e.Item.FindControl("cmdComments");
            if (cmd != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmd.CommandName))
                    cmd.Visible = false;

                cmd.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionTMSAMatrixComments.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&contentid=" + General.GetNullableGuid(ViewState["CONTENTID"].ToString()) + "'); return false;");
            }

            LinkButton lblid1 = (LinkButton)e.Item.FindControl("lblid1");
            if (lblid1 != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, lblid1.CommandName))
                    lblid1.Visible = false;

                lblid1.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionTMSAMatrixObjectiveEvidenceUpdate.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&contentid=" + General.GetNullableGuid(ViewState["CONTENTID"].ToString()) + "',false,500,300); return false;");
            }

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            LinkButton BPGAtt = (LinkButton)e.Item.FindControl("BPGAtt");
            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                HtmlGenericControl html = new HtmlGenericControl();
                if (drv["FLDISATTACHMENT"].ToString() == "0")
                {
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }
                else
                {
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fa-paperclip\"></i></span>";
                    att.Controls.Add(html);
                }
                //    att.ImageUrl = Session["images"] + "/no-attachment.png";
                att.Attributes.Add("onclick", "javascript:openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.QUALITY + "&type=SHIPBOARDEVIDENCE&cmdname=SHIPBOARDEVIDENCEUPLOAD&VESSELID=0" + "'); return true;");
            }

            if (BPGAtt != null)
            {
                BPGAtt.Visible = SessionUtil.CanAccess(this.ViewState, BPGAtt.CommandName);
                HtmlGenericControl html = new HtmlGenericControl();
                if (drv["FLDISBPGATTACHMENT"].ToString() == "0")
                {
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fa-paperclip-na\"></i></span>";
                    BPGAtt.Controls.Add(html);
                }
                else
                {
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fa-paperclip\"></i></span>";
                    BPGAtt.Controls.Add(html);
                }
                //    att.ImageUrl = Session["images"] + "/no-attachment.png";
                BPGAtt.Attributes.Add("onclick", "javascript:openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.QUALITY + "&type=BPGEXAMPLE&cmdname=SHIPBOARDEVIDENCEUPLOAD&VESSELID=0" + "'); return true;");
            }

            ImageButton cmdClient = (ImageButton)e.Item.FindControl("cmdClientComments");
            if (cmdClient != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdClient.CommandName))
                    cmdClient.Visible = false;

                cmdClient.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionTMSAClientBPGComments.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&contentid=" + General.GetNullableGuid(ViewState["CONTENTID"].ToString()) + "'); return false;");
            }

            RadCheckBox chkSelect = (RadCheckBox)e.Item.FindControl("chkSelect");

            if (cmdedit != null)
            {
                if (ViewState["LOCK"].ToString() == "1")
                {
                    cmdedit.Visible = false;
                    chkSelect.Enabled = false;
                }
                else
                    cmdedit.Visible = true;

            }

            LinkButton cmdOfficeChecks = (LinkButton)e.Item.FindControl("cmdOfficeChecks");
            LinkButton cmdarchive = (LinkButton)e.Item.FindControl("cmdarchive");

            if (cmdOfficeChecks != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdOfficeChecks.CommandName))
                    cmdOfficeChecks.Visible = false;

                cmdOfficeChecks.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionTMSAMatrixOfficeChecks.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&contentid=" + General.GetNullableGuid(ViewState["CONTENTID"].ToString()) + "&inspectionid=" + General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()) + "'); return false;");

                if (drv["FLDISPLANNED"].ToString() == "0")
                {
                    cmdOfficeChecks.Visible = false;
                    //chkSelect.Enabled = false;
                }
                else
                    cmdOfficeChecks.Visible = true;
            }

            if (drv["FLDFORMPOSTERCHECKLISTS"].ToString() != "" || drv["FLDPROCEDURES"].ToString() != "")
            {
                DataSet dss = PhoenixInspectionTMSAMatrix.FormPosterList(ViewState["CONTENTID"] == null ? null : General.GetNullableGuid(ViewState["CONTENTID"].ToString()),
                                                    General.GetNullableGuid(ViewState["CATEGORYID"].ToString()), 0);
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

                        if (dr["FLDTYPE"].ToString() == "2")
                            hl.Attributes.Add("onclick", "openNewWindow('codehelp1', '', 'DocumentManagement/DocumentManagementDocumentGeneral.aspx?showall=0&DOCUMENTID=" + dr["FLDFORMPOSTERID"].ToString() + "');return false;");
                        else if (dr["FLDTYPE"].ToString() == "3")
                            hl.Attributes.Add("onclick", "openNewWindow('codehelp1', '', 'DocumentManagement/DocumentManagementDocumentSectionContentView.aspx?callfrom=documenttree&SECTIONID=" + dr["FLDFORMPOSTERID"].ToString() + "');return false;");
                        else if (dr["FLDTYPE"].ToString() == "5")
                        {
                            hl.Attributes.Add("onclick", "openNewWindow('codehelp1', '', 'StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&FORMID=" + dr["FLDFORMPOSTERID"].ToString() + "');return false;");
                        }
                        else if (dr["FLDTYPE"].ToString() == "6")
                        {
                            hl.Target = "_blank";
                            hl.NavigateUrl = "../Common/download.aspx?formid=" + dr["FLDFORMPOSTERID"].ToString();
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
            }

            if (drv["FLDFORMREPORTLIST"].ToString() != "")
            {

                DataSet dsreports = PhoenixInspectionTMSAMatrix.FormReportList(ViewState["CONTENTID"] == null ? null : General.GetNullableGuid(ViewState["CONTENTID"].ToString()),
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
                gvTMSAMatrix.DataSource = dt;
                gvTMSAMatrix.DataSource = dtcolumn;
            }
            else
            {
                gvTMSAMatrix.DataSource = dt;
            }
            for (int i = noofcolumns + 7; i < gvTMSAMatrix.Columns.Count; i++)
            {
                gvTMSAMatrix.Columns[i].Visible = false;
            }
            gvTMSAMatrix.DataBind();

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
                    if (dr[columnheadertext].ToString() == "Stage")
                    {
                        field.HeaderStyle.Width = 50;
                        field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    }
                    else
                    {
                        field.HeaderStyle.Width = 200;
                        field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    }
                    field.ItemTemplate = new GridViewTemplate(ListItemType.Item, columnheadervalue, "Label");
                    //field.FooterTemplate = new GridViewTemplate(ListItemType.Footer, j, dr[columnheadertext].ToString(), "RadTextBox");                    
                    //gvTMSAMatrix.Columns.Add(field); 
                    gvTMSAMatrix.Columns.AddAt(i, field);
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
                    gvTMSAMatrix.Columns.Add(field);
                }

            }
        }

        return dt.Rows.Count;
    }

    protected void confirmTMSAReview_Click(object sender, EventArgs e)
    {
        try
        {
            if (General.GetNullableGuid(lblSelectedNode.Text) != null)
            {
                PhoenixInspectionTMSAMatrix.PublishTMSAMatrixCategory(
                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                        , General.GetNullableGuid(lblSelectedNode.Text)
                                        , General.GetNullableDateTime(DateTime.Now.ToString())
                                        );

                //Filter.CurrentSelectedComments = null;
                ucStatus.Text = "Document published.";
                BindData();
                DocumentCategoryEdit();
                setMenu();
                gvTMSAMatrix.Rebind();
            }
            else
            {
                ucError.ErrorMessage = "Select the category";
                ucError.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void confirmTMSAUnlock_Click(object sender, EventArgs e)
    {
        try
        {
            if (General.GetNullableGuid(lblSelectedNode.Text) != null)
            {
                PhoenixInspectionTMSAMatrix.UnlockTMSAMatrixCategory(
                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                        , General.GetNullableGuid(lblSelectedNode.Text)
                                        );

                //Filter.CurrentSelectedComments = null;
                ucStatus.Text = "Document unlocked to revise.";
                BindData();
                DocumentCategoryEdit();
                setMenu();
                gvTMSAMatrix.Rebind();
            }
            else
            {
                ucError.ErrorMessage = "Select the category";
                ucError.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
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


    protected void CreateDynamicTextBox()
    {
        if (txtnoofcolumns.Text != "")
        {
            int n = int.Parse(txtnoofcolumns.Text);
            for (int i = 1; i <= n; i++)
            {
                Literal lit = new Literal();
                lit.Text = "Column Header " + i;
                Pnlcolumnlabel.Controls.Add(lit);
                Literal lit2 = new Literal();
                lit2.Text = "</br></br>";
                Pnlcolumnlabel.Controls.Add(lit2);
                RadTextBox MyTextBox = new RadTextBox();
                //Assigning the textbox ID name  
                MyTextBox.ID = "txtbox" + i.ToString();
                MyTextBox.Width = 180;
                MyTextBox.Height = 20;
                MyTextBox.Font.Name = "Tahoma";
                MyTextBox.Font.Size = 8;
                MyTextBox.TextMode = InputMode.SingleLine;
                //this.Controls.Add(MyTextBox);
                pnlcolumns.Controls.Add(MyTextBox);
                Literal lit1 = new Literal();
                lit1.Text = "</br></br>";
                pnlcolumns.Controls.Add(lit1);

                if (lblSelectedNode.Text != "")
                {
                    DataTable ds = new DataTable();
                    ds = PhoenixInspectionTMSAMatrix.EditTMSAMatrixColumnHeaders(General.GetNullableGuid(lblSelectedNode.Text));
                    if (ds.Rows.Count > 0)
                    {
                        MyTextBox.Text = ds.Rows[0][i].ToString();
                    }

                }
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
                Reset();
            }

            if (CommandName.ToUpper().Equals("SAVE"))
            {

                if (lblCategoryId.Text == "")
                {
                    //insert
                    if (!IsValidDocumentCategory())
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixInspectionTMSAMatrix.InsertTMSAMatrixCategory(
                                                         General.GetNullableGuid(lblSelectedNode.Text),
                                                         txtDocumentCategory.Text.Trim(),
                                                         chkActiveyn.Checked == true ? General.GetNullableInteger("1") : General.GetNullableInteger("0"),
                                                         txtCategoryNumber.Text,
                                                         txtcategoryshortcode.Text,
                                                         General.GetNullableInteger(ucCompany.SelectedCompany),
                                                         General.GetNullableInteger(txtnoofcolumns.Text)
                                                         );
                    Reset();
                    ucStatus.Text = "Category is added.";

                    //PhoenixInspectionTMSAMatrix.InsertTMSAMatrixColumnHeaders(General.GetNullableGuid(lblSelectedNode.Text), General.GetNullableInteger(txtnoofcolumns.Text),
                    //    General.GetNullableInteger(ucCompany.SelectedCompany), null, null);
                    CreateDynamicTextBox();

                }
                else
                {

                    //update
                    PhoenixInspectionTMSAMatrix.UpdateTMSAMatrixCategory(
                                                        new Guid(lblSelectedNode.Text),
                                                        txtDocumentCategory.Text.Trim(),
                                                        chkActiveyn.Checked == true ? 1 : 0,
                                                        txtCategoryNumber.Text,
                                                        General.GetNullableInteger(ucCompany.SelectedCompany),
                                                        General.GetNullableInteger(txtnoofcolumns.Text)
                                                        );

                    int n = int.Parse(txtnoofcolumns.Text);
                    if (n > 0)
                    {
                        string column = "";
                        string columnvalue = "";
                        for (int i = 1; i <= n; i++)
                        {
                            TextBox MyTextBox = new TextBox();
                            if (MyTextBox != null)
                            {
                                string ss = Request.Form["txtbox" + (i).ToString()].ToString();
                                column = "column" + i.ToString();
                                columnvalue = ss;
                                if (MyTextBox != null)
                                {
                                    PhoenixInspectionTMSAMatrix.InsertTMSAMatrixColumnHeaders(new Guid(lblSelectedNode.Text), General.GetNullableInteger(txtnoofcolumns.Text),
                                        General.GetNullableInteger(ucCompany.SelectedCompany), General.GetNullableString(column), General.GetNullableString(columnvalue));

                                }
                            }
                        }



                    }
                    ucStatus.Text = "Category updated.";
                    CreateDynamicTextBox();
                    BindData();
                }
                BindDocumentCategoryTree();
            }
            if (CommandName.ToUpper().Equals("DELETE"))
            {
                if (string.IsNullOrEmpty(lblSelectedNode.Text) || lblSelectedNode.Text.ToString().ToUpper() == "ROOT")
                {
                    ucError.ErrorMessage = "Root cannot be deleted.";
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    PhoenixInspectionTMSAMatrix.DeleteTMSAMatrixCategory(new Guid(lblSelectedNode.Text));
                    Reset();
                    lblSelectedNode.Text = "";
                    ucStatus.Text = "Category deleted.";
                }
                BindDocumentCategoryTree();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public bool IsValidDocumentCategory()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(txtDocumentCategory.Text.Trim()))
            ucError.ErrorMessage = "Category name is required.";

        if (string.IsNullOrEmpty(txtCategoryNumber.Text.Trim()))
            ucError.ErrorMessage = "Category number is required.";

        if (General.GetNullableInteger(ucCompany.SelectedCompany) == null)
            ucError.ErrorMessage = "Company is required.";

        return (!ucError.IsError);
    }
    protected void DocumentCategoryEdit()
    {
        if (!string.IsNullOrEmpty(lblSelectedNode.Text) && lblSelectedNode.Text.ToString().ToUpper() != "ROOT")
        {
            ucCompany.SelectedCompany = "";
            DataSet ds = PhoenixInspectionTMSAMatrix.TMSAMatrixCategoryEdit(new Guid(lblSelectedNode.Text));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["CATEGORYID"] = ds.Tables[0].Rows[0]["FLDCATEGORYID"].ToString();

                txtDocumentCategory.Text = ds.Tables[0].Rows[0]["FLDCATEGORYNAME"].ToString();
                chkActiveyn.Checked = ds.Tables[0].Rows[0]["FLDACTIVEYN"].ToString() == "1" ? true : false;
                lblCategoryId.Text = ds.Tables[0].Rows[0]["FLDCATEGORYID"].ToString();
                txtCategoryNumber.Text = ds.Tables[0].Rows[0]["FLDCATEGORYNUMBER"].ToString();
                //rListAdd.SelectedValue = ds.Tables[0].Rows[0]["FLDACCESSLEVEL"].ToString();

                ucCompany.SelectedCompany = ds.Tables[0].Rows[0]["FLDCOMPANYID"].ToString();
                ViewState["INSPECTIONID"] = ds.Tables[0].Rows[0]["FLDINSPECTIONID"].ToString();
                txtnoofcolumns.Text = ds.Tables[0].Rows[0]["FLDNOOFCOLUMNS"].ToString();
                ViewState["NOOFCOLUMNS"] = ds.Tables[0].Rows[0]["FLDNOOFCOLUMNS"].ToString();
                ViewState["NOOFSTAGES"] = ds.Tables[0].Rows[0]["FLDNOOFSTAGES"].ToString();
                ViewState["LOCK"] = ds.Tables[0].Rows[0]["FLDISLOCK"].ToString();

            }
        }
        else
        {
            txtDocumentCategory.Text = "";
            chkActiveyn.Checked = false;
            lblCategoryId.Text = "";
            ucCompany.SelectedCompany = "";
        }
        setMenu();
    }
    private void Reset()
    {
        txtDocumentCategory.Text = "";
        chkActiveyn.Checked = true;
        lblCategoryId.Text = "";
        txtCategoryNumber.Text = "";
        ucCompany.SelectedCompany = "";
        ucCompany.SelectedCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString();
        //txtnoofcolumns.Text = "";
        gvTMSAMatrix.Visible = false;
    }

    protected void ddlCompany_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        gvTMSAMatrix.Visible = true;
        DocumentCategoryEdit();
        CreateDynamicTextBox();
        BindData();
        //configpart.Visible = false;
    }

    protected void ddldeptlist_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        gvTMSAMatrix.Visible = true;
        DocumentCategoryEdit();
        CreateDynamicTextBox();
        BindData();
        //configpart.Visible = false;
    }

    protected void chkSelect_Click(object sender, EventArgs e)
    {
        ArrayList SelectedSections = new ArrayList();
        Guid index = new Guid();

        foreach (GridDataItem item in gvTMSAMatrix.Items)
        {
            if (item.GetDataKeyValue("FLDCONTENTID").ToString() != "")
            {
                index = new Guid(item.GetDataKeyValue("FLDCONTENTID").ToString());

                PhoenixInspectionTMSAMatrix.ComplianceUpdate(General.GetNullableGuid(index.ToString()),
                                                (((RadCheckBox)item.FindControl("chkSelect")).Checked == true) ? 1 : 0);
            }
        }
        if (Request.QueryString["categoryid"] != null || ViewState["CONTENTID"] != null)
        {
            BindData();
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
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
}