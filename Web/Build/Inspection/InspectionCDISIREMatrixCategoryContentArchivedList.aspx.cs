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

public partial class Inspection_InspectionCDISIREMatrixCategoryContentArchivedList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        lblSelectedNode.Visible = false;
        lblCategoryId.Visible = false;
        configpart.Visible = false;

        //PhoenixToolbar toolbar = new PhoenixToolbar();
        //toolbar.AddFontAwesomeButton("../Inspection/InspectionCDISIREMatrixCategoryContentList.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        ////toolbar.AddImageLink("javascript:parent.Openpopup('Filter','','../Inspection/InspectionCDISIREMatrixContentAdd.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&noofcolumns=" + General.GetNullableInteger(ViewState["NOOFCOLUMNS"].ToString()) + "'); return false;", "Add", "add.png", "ADD");
        ////toolbar.AddImageLink("javascript:Openpopup('codehelp1','','InspectionIncidentAdd.aspx');return true;", "Add New", "add.png", "ADD");
        //toolbar.AddFontAwesomeButton("../Inspection/InspectionCDISIREMatrixCategoryContentList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
        //toolbar.AddFontAwesomeButton("../Inspection/InspectionCDISIREMatrixCategoryContentList.aspx", "Publish", "<i class=\"fa fa-check-square\"></i>", "PUBLISH");
        //toolbar.AddFontAwesomeButton("../Inspection/InspectionCDISIREMatrixCategoryContentList.aspx", "Revisions", "<i class=\"fas fa-receipt\"></i>", "REVISIONS");
        //MenuPhoenixQuery.MenuList = toolbar.Show();
        //MenuPhoenixQuery.Visible = true;

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        //toolbarmain.AddButton("Delete", "DELETE", ToolBarDirection.Right);
        //toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        //toolbarmain.AddButton("New", "NEW", ToolBarDirection.Right);

        MenuDocumentCategoryMain.AccessRights = this.ViewState;
        MenuDocumentCategoryMain.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            confirmReview.Attributes.Add("style", "display:none;");
            confirmCDISIREUnlock.Attributes.Add("style", "display:none;");

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["CATEGORYID"] = "";
            ViewState["NOOFCOLUMNS"] = "";
            ViewState["INSPECTIONID"] = "";
            ViewState["ERROR"] = "";
            ViewState["LOCK"] = "";


            if (Request.QueryString["categoryid"] != null)
                lblSelectedNode.Text = Request.QueryString["categoryid"].ToString();

            if (Request.QueryString["noofcolumns"] != null)
                txtnoofcolumns.Text = Request.QueryString["noofcolumns"].ToString();

            if (Request.QueryString["inspectionid"] != null && Request.QueryString["inspectionid"].ToString() != string.Empty)
                ViewState["INSPECTIONID"] = Request.QueryString["inspectionid"].ToString();


            ucCompany.Enabled = false;
            ucCompany.SelectedCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString();
            BindDocumentCategoryTree();
            DocumentCategoryEdit();
            CreateDynamicTextBox();
            MenuPhoenixQuery.Visible = false;
            BindData();
            MenuPhoenixQuery.Visible = true;
            BindCompany();
            BindInspectionReferenceNumber();
        }
        setMenu();
        if ((Request.QueryString["categoryid"] != null || ViewState["CONTENTID"] != null) && ViewState["ERROR"].ToString() == "1")
        {
            BindData();
            ViewState["ERROR"] = "";
        }

    }

    protected void BindInspectionReferenceNumber()
    {
        DataSet ds = PhoenixInspectionCDISIREMatrix.ListCDISIREMatrixReferenceNumberList(General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        ddlRefNo.DataSource = ds.Tables[0];
        ddlRefNo.DataTextField = "FLDREFERENCENUMBER";
        ddlRefNo.DataValueField = "FLDINSPECTIONSCHEDULEID";
        ddlRefNo.DataBind();
        ddlRefNo.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
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

        //if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
        //{
        //    toolbar.AddFontAwesomeButton("../Inspection/InspectionCDISIREMatrixCategoryContentList.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        //}
        //toolbar.AddImageLink("javascript:parent.Openpopup('Filter','','../Inspection/InspectionCDISIREMatrixContentAdd.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&noofcolumns=" + General.GetNullableInteger(ViewState["NOOFCOLUMNS"].ToString()) + "'); return false;", "Add", "add.png", "ADD");
        //toolbar.AddImageLink("javascript:Openpopup('codehelp1','','InspectionIncidentAdd.aspx');return true;", "Add New", "add.png", "ADD");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionCDISIREMatrixCategoryContentList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
        //if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
        //{
        //    if (ViewState["LOCK"].ToString() == "0")
        //    {
        //        toolbar.AddImageButton("../Inspection/InspectionCDISIREMatrixCategoryContentList.aspx", "Publish", "approve.png", "PUBLISH");
        //    }
        //    toolbar.AddImageLink("javascript:parent.openNewWindow('Filter','','Inspection/InspectionCDISIREMatrixDistribution.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&inspectionid=" + General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()) + "'); return false;", "Distribute", "vessel.png", "DISTRIBUTE");
        //    toolbar.AddImageButton("../Inspection/InspectionCDISIREMatrixCategoryContentList.aspx", "Revisions", "requisition.png", "REVISIONS");
        //}
        //if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
        //{
        //    toolbar.AddImageLink("javascript:parent.openNewWindow('Filter','','Inspection/InspectionCDISIREVerificationSummary.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&inspectionid=" + General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()) + "'); return false;", "Verification Summary", "checklist.png", "SUMMARY");
        //}
        //if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
        //{
        //    if (ViewState["LOCK"].ToString() == "1")
        //    {
        //        toolbar.AddImageButton("../Inspection/InspectionCDISIREMatrixCategoryContentList.aspx", "Unlock", "unlock.png", "UNLOCK");
        //    }
        //}
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

            if (ViewState["CATEGORYID"].ToString() != "" && ViewState["INSPECTIONID"] != null)
            {
                DataTable dt = new DataTable();
                dt = PhoenixInspectionCDISIREMatrix.EditCDISIREMatrixColumnHeaders(General.GetNullableGuid(lblSelectedNode.Text), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                if (dt.Rows[0]["FLDCOLUMN2"].ToString() != "" || dt.Rows[0]["FLDCOLUMN3"].ToString() != "")
                {
                    Response.Redirect("../Inspection/InspectionCDISIREMatrixContentAdd.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&noofcolumns=" + General.GetNullableInteger(ViewState["NOOFCOLUMNS"].ToString()) + "&inspectionid=" + General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()) + "&action=add");
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
            RadWindowManager2.RadConfirm("Are you sure you want to publish the document.? Y/N", "confirmReview", 320, 150, null, "Confirm");
            if (lblSelectedNode.Text != "")
            {
                BindData();
            }
        }

        if (CommandName.ToUpper().Equals("UNLOCK"))
        {
            if (ViewState["CATEGORYID"].ToString() != "" && ViewState["CATEGORYID"] != null)
            {
                RadWindowManager2.RadConfirm("Are you sure you want to unlock the document.? Y/N", "confirmCDISIREUnlock", 320, 150, null, "ConfirmUnlock");
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
            Response.Redirect("../Inspection/InspectionCDISIREMatrixCategoryRevisions.aspx?CATEGORYID=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()));
        }
    }

    protected void ShowExcel()
    {

        DataTable dt = new DataTable();
        dt = PhoenixInspectionCDISIREMatrix.EditCDISIREMatrixColumnHeaders(General.GetNullableGuid(lblSelectedNode.Text), PhoenixSecurityContext.CurrentSecurityContext.VesselID);

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

        columns = columns + ",FLDPROCEDURES,FLDOBJECTIVEEVIDENCE,FLDDEPARTMENTNAMELIST";
        captions = captions + ",Procedures, Objective Evidence,Responsibility";

        string[] alColumns = columns.Split(',');
        string[] alCaptions = captions.Split(',');


        DataTable dtcolumn = new DataTable();

        dtcolumn = PhoenixInspectionCDISIREMatrix.CDISIREMatrixCategoryContentSearch(General.GetNullableGuid(lblSelectedNode.Text), General.GetNullableGuid(ddlCompany.SelectedValue), PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        if (dtcolumn.Rows.Count > 0 && lblSelectedNode.Text != "")
        {
            ShowQuery(General.GetNullableGuid(lblSelectedNode.Text), dt, dtcolumn);
        }
        else
        {
            DataTable dt1 = dtcolumn;
            gvCDISIREMatrix.DataSource = dtcolumn;
            //gvCDISIREMatrix.Visible = false;
            //dtcolumn.Rows.Clear();
            //DataTable dt1 = dtcolumn;
            //ShowNoRecordsFound(dt1, gvCDISIREMatrix);
        }
        ShowExcel("CDI/SIRE Matrix", dtcolumn, alColumns, alCaptions, null, null);
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

    public override void VerifyRenderingInServerForm(Control control)
    {
    }

    private void BindDocumentCategoryTree()
    {

        DataSet ds = new DataSet();

        int companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

        ds = PhoenixInspectionCDISIREMatrix.ListTreeCDISIREMatrixCategory(companyid, PhoenixSecurityContext.CurrentSecurityContext.VesselID);

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

        if (lblSelectedNode.Text == "_Root")
        {

            lblSelectedNode.Text = "";
            lblCategoryId.Text = "";
            txtCategoryNumber.Text = "";
            txtDocumentCategory.Text = "";
            txtnoofcolumns.Text = "";
            MenuPhoenixQuery.Visible = false;
            gvCDISIREMatrix.Visible = false;
        }
        else
        {
            gvCDISIREMatrix.Visible = true;
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
                ds = PhoenixInspectionCDISIREMatrix.CDISIREMatrixCategoryEdit(new Guid(lblSelectedNode.Text), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["INSPECTIONID"] = ds.Tables[0].Rows[0]["FLDINSPECTIONID"].ToString();
                    if (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["FLDPARENTCATEGORYID"].ToString()))
                    {
                        txtnoofcolumns.Enabled = true;
                    }
                    else
                        txtnoofcolumns.Enabled = false;
                }
            }
        }
        setMenu();
    }

    //protected void rblType_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    BindData();
    //}

    protected void gvCDISIREMatrix_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (Request.QueryString["inspectionid"] != null && ddlRefNo.SelectedValue != null)
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
            dt = PhoenixInspectionCDISIREMatrix.EditCDISIREMatrixColumnHeaders(General.GetNullableGuid(lblSelectedNode.Text), PhoenixSecurityContext.CurrentSecurityContext.VesselID);


            DataTable dtcolumn = new DataTable();

            dtcolumn = PhoenixInspectionCDISIREMatrix.CDISIREMatrixCategoryContentArchivedSearch(General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()), General.GetNullableGuid(ddlRefNo.SelectedValue));

            if (dtcolumn.Rows.Count > 0)
            {
                ShowQuery(General.GetNullableGuid(lblSelectedNode.Text), dt, dtcolumn);
            }
            else
            {
                gvCDISIREMatrix.DataSource = dtcolumn;
                gvCDISIREMatrix.DataBind();
                //gvCDISIREMatrix.Visible = false;
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

        try
        {

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

            if (e.CommandName.ToUpper().Equals("CHANGEPAGESIZE"))
                return;

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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            BindData();
            ViewState["ERROR"] = "1";
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
            DataRowView drt = (DataRowView)e.Item.DataItem;
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
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

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

            LinkButton lblid1 = (LinkButton)e.Item.FindControl("lblid1");
            if (lblid1 != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, lblid1.CommandName))
                    lblid1.Visible = false;

                lblid1.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionCDISIREMatrixObjectiveEvidenceUpdate.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&contentid=" + General.GetNullableGuid(ViewState["CONTENTID"].ToString()) + "',false,500,300); return false;");
            }

            ImageButton cmdClient = (ImageButton)e.Item.FindControl("cmdClientComments");
            if (cmdClient != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdClient.CommandName))
                    cmdClient.Visible = false;

                cmdClient.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionCDISIREClientBPGComments.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&contentid=" + General.GetNullableGuid(ViewState["CONTENTID"].ToString()) + "&inspectionid=" + General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()) + "'); return false;");
            }

            if (edit != null)
            {
                if (ViewState["LOCK"].ToString() == "1")
                {
                    edit.Visible = false;
                    //chkSelect.Enabled = false;
                }
                else
                    edit.Visible = true;
            }

            LinkButton cmdOnboardChecks = (LinkButton)e.Item.FindControl("cmdOnboardChecks");
            LinkButton cmdarchive = (LinkButton)e.Item.FindControl("cmdarchive");
            RadCheckBox chkSelect = (RadCheckBox)e.Item.FindControl("chkSelect");


            if (cmdOnboardChecks != null && PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdOnboardChecks.CommandName))
                    cmdOnboardChecks.Visible = false;

                cmdOnboardChecks.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionCDISIREMatrixOnboardChecks.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&contentid=" + General.GetNullableGuid(ViewState["CONTENTID"].ToString()) + "&inspectionid=" + General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()) + "&dtkey=" + drt["FLDDTKEY"].ToString() + "'); return false;");

                edit.Visible = false;
                del.Visible = false;
                chkSelect.Enabled = false;

            }
            else
            {
                cmdOnboardChecks.Visible = false;
            }

            if (cmdarchive != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdarchive.CommandName))
                    cmdarchive.Visible = false;

                cmdarchive.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionCDISIREInspectionArchivedList.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&contentid=" + General.GetNullableGuid(ViewState["CONTENTID"].ToString()) + "&inspectionid=" + General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()) + "&dtkey=" + drt["FLDDTKEY"].ToString() + "'); return false;");
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
                                                        General.GetNullableGuid(ViewState["CATEGORYID"].ToString()), 0, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
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
                                                        General.GetNullableGuid(ViewState["CATEGORYID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
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



    //void cb_CheckedChanged(object sender, EventArgs e)
    //{
    //    CheckBox c = (CheckBox)sender;
    //    RadioButtonList rbl = (RadioButtonList)sender;
    //    if (c.Checked == false)
    //    {
    //        PhoenixInspectionRiskAssessmentProcess.FormPosterDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
    //            new Guid(ViewState["RISKASSESSMENTPROCESSID"].ToString()), new Guid(c.ID), int.Parse(rbl.SelectedValue));

    //        string txt = "";
    //        if (rbl.SelectedValue == "0")
    //        {
    //            txt = "Forms/Posters/Checklists";
    //        }
    //        else if (rbl.SelectedValue == "1")
    //        {
    //            txt = "Procedures";
    //        }
    //        else if (rbl.SelectedValue == "2")
    //        {
    //            txt = "Contingency/Emergency";
    //        }

    //        ucStatus.Text = txt + " deleted.";
    //        //BindFormPosters();
    //    }
    //}


    //private void ShowNoRecordsFound(DataTable dt, RadGrid gv)
    //{
    //    dt.Rows.Add(dt.NewRow());
    //    gv.DataSource = dt;
    //    gv.DataBind();

    //    int colcount = gv.Columns.Count;
    //    gv.Items[0].Cells.Clear();
    //    gv.Items[0].Cells.Add(new TableCell());
    //    gv.Items[0].Cells[0].ColumnSpan = colcount;
    //    gv.Items[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
    //    gv.Items[0].Cells[0].ForeColor = System.Drawing.Color.Red;
    //    gv.Items[0].Cells[0].Font.Bold = true;
    //    gv.Items[0].Cells[0].Text = "NO RECORDS FOUND";
    //}

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
            for (int i = noofcolumns + 7; i < gvCDISIREMatrix.Columns.Count; i++)
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
                    if (columnheader == "FLDCOLUMN1")
                    {
                        field.HeaderStyle.Width = 60;
                    }
                    else
                        field.HeaderStyle.Width = 200;
                    field.ItemTemplate = new GridViewTemplate(ListItemType.Item, columnheadervalue, "Label");
                    //field.FooterTemplate = new GridViewTemplate(ListItemType.Footer);
                    //gvCDISIREMatrix.Columns.Add(field); 
                    gvCDISIREMatrix.Columns.AddAt(i, field);
                }
                //TemplateField procname = new TemplateField();
                //procname.HeaderTemplate = new GridViewTemplate(ListItemType.Header, dr["FLDFORMPOSTERCHECKLISTS"].ToString());
                //procname.ItemTemplate = new GridViewTemplate(ListItemType.Item, "FLDFORMPOSTERCHECKLISTS", "Label");
                //procname.FooterTemplate = new GridViewTemplate(ListItemType.Footer);
                //gvCDISIREMatrix.Columns.Add(procname);
                //TemplateField objevidence = new TemplateField();
                //objevidence.HeaderTemplate = new GridViewTemplate(ListItemType.Header, dr["FLDOBJECTIVEEVIDENCE"].ToString());
                //objevidence.ItemTemplate = new GridViewTemplate(ListItemType.Item, "FLDOBJECTIVEEVIDENCE", "Label");
                //objevidence.FooterTemplate = new GridViewTemplate(ListItemType.Footer);
                //gvCDISIREMatrix.Columns.Add(objevidence);
                //TemplateField action = new TemplateField();
                //action.HeaderTemplate = new GridViewTemplate(ListItemType.Header, dr["FLDACTION"].ToString());
                //action.ItemTemplate = new GridViewTemplate(ListItemType.Item, "FLDACTION", "LinkButton");
                //action.FooterTemplate = new GridViewTemplate(ListItemType.Footer, "FLDFOOTER", "LinkButton");
                //gvCDISIREMatrix.Columns.Add(action);
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
                //TemplateField procname = new TemplateField();
                //procname.HeaderTemplate = new GridViewTemplate(ListItemType.Header, dr["FLDPROCEDURENAME"].ToString());
                //procname.ItemTemplate = new GridViewTemplate(ListItemType.Item, dr["FLDPROCEDURENAME"].ToString(), "Label");
                //procname.FooterTemplate = new GridViewTemplate(ListItemType.Footer, dr["FLDPROCEDURENAME"].ToString(), "TextBox");
                //gvCDISIREMatrix.Columns.Add(procname);
                //TemplateField objevidence = new TemplateField();
                //objevidence.HeaderTemplate = new GridViewTemplate(ListItemType.Header, dr["FLDOBJECTIVEEVIDENCE"].ToString());
                //objevidence.ItemTemplate = new GridViewTemplate(ListItemType.Item, dr["FLDOBJECTIVEEVIDENCE"].ToString(), "Label");
                //objevidence.FooterTemplate = new GridViewTemplate(ListItemType.Footer, dr["FLDOBJECTIVEEVIDENCE"].ToString(), "TextBox");
                //gvCDISIREMatrix.Columns.Add(objevidence);
                // TemplateField action = new TemplateField();
                //action.HeaderTemplate = new GridViewTemplate(ListItemType.Header, dr["FLDACTION"].ToString());
                //action.ItemTemplate = new GridViewTemplate(ListItemType.Item, dr["FLDACTION"].ToString(), "Label");
                //action.FooterTemplate = new GridViewTemplate(ListItemType.Footer, dr["FLDADD"].ToString(), "LinkButton");
                //gvCDISIREMatrix.Columns.Add(action);
            }
        }

        //var columntomove1 = gvCDISIREMatrix.Columns[0];
        //gvCDISIREMatrix.Columns.RemoveAt(0);
        //gvCDISIREMatrix.Columns.Insert(noofcolumns + 2, columntomove1);

        //var columntomove = gvCDISIREMatrix.Columns[0];
        //gvCDISIREMatrix.Columns.RemoveAt(0);
        //gvCDISIREMatrix.Columns.Insert(noofcolumns + 2, columntomove);

        //var columntomove2 = gvCDISIREMatrix.Columns[0];
        //gvCDISIREMatrix.Columns.RemoveAt(0);
        //gvCDISIREMatrix.Columns.Insert(noofcolumns + 2, columntomove2);

        return dt.Rows.Count;
    }

    protected void confirmReview_Click(object sender, EventArgs e)
    {
        try
        {

            if (General.GetNullableGuid(lblSelectedNode.Text) != null)
            {
                PhoenixInspectionCDISIREMatrix.PublishCDISIREMatrixCategory(
                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                            , General.GetNullableGuid(lblSelectedNode.Text)
                                            , General.GetNullableDateTime(DateTime.Now.ToString())
                                            , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                            );

                //Filter.CurrentSelectedComments = null;
                ucStatus.Text = "Document published.";
                BindData();
                DocumentCategoryEdit();
                setMenu();
                gvCDISIREMatrix.Rebind();
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

    protected void confirmCDISIREUnlock_Click(object sender, EventArgs e)
    {
        try
        {
            if (General.GetNullableGuid(lblSelectedNode.Text) != null)
            {
                PhoenixInspectionCDISIREMatrix.UnlockCDISIREMatrixCategory(
                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                        , General.GetNullableGuid(lblSelectedNode.Text)
                                        , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                        );

                //Filter.CurrentSelectedComments = null;
                ucStatus.Text = "Document unlocked to revise.";
                BindData();
                DocumentCategoryEdit();
                setMenu();
                gvCDISIREMatrix.Rebind();
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

    //protected void gvCDISIREMatrix_RowCommand_RowCommand(object sender, GridViewCommandEventArgs gce)
    //{
    //    GridView _gridView = (GridView)sender;
    //    int nRow = int.Parse(gce.CommandArgument.ToString());

    //    if (gce.CommandName.ToUpper().Equals("EDITROW"))
    //    {
    //        Response.Redirect("../Inspection/InspectionRAJobHazardAnalysis.aspx", false);
    //    }
    //}


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
                    ds = PhoenixInspectionCDISIREMatrix.EditCDISIREMatrixColumnHeaders(General.GetNullableGuid(lblSelectedNode.Text), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
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

                    PhoenixInspectionCDISIREMatrix.InsertCDISIREMatrixCategory(
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

                    //PhoenixInspectionCDISIREMatrix.InsertCDISIREMatrixColumnHeaders(General.GetNullableGuid(lblSelectedNode.Text), General.GetNullableInteger(txtnoofcolumns.Text),
                    //    General.GetNullableInteger(ucCompany.SelectedCompany), null, null);
                    CreateDynamicTextBox();

                }
                else
                {

                    //update
                    PhoenixInspectionCDISIREMatrix.UpdateCDISIREMatrixCategory(
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
                                    PhoenixInspectionCDISIREMatrix.InsertCDISIREMatrixColumnHeaders(new Guid(lblSelectedNode.Text), General.GetNullableInteger(txtnoofcolumns.Text),
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
                    PhoenixInspectionCDISIREMatrix.DeleteCDISIREMatrixCategory(new Guid(lblSelectedNode.Text));
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
            DataSet ds = PhoenixInspectionCDISIREMatrix.CDISIREMatrixCategoryEdit(new Guid(lblSelectedNode.Text), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["CATEGORYID"] = ds.Tables[0].Rows[0]["FLDCATEGORYID"].ToString();
                ViewState["INSPECTIONID"] = ds.Tables[0].Rows[0]["FLDINSPECTIONID"].ToString();
                txtDocumentCategory.Text = ds.Tables[0].Rows[0]["FLDCATEGORYNAME"].ToString();
                chkActiveyn.Checked = ds.Tables[0].Rows[0]["FLDACTIVEYN"].ToString() == "1" ? true : false;
                lblCategoryId.Text = ds.Tables[0].Rows[0]["FLDCATEGORYID"].ToString();
                txtCategoryNumber.Text = ds.Tables[0].Rows[0]["FLDCATEGORYNUMBER"].ToString();
                //rListAdd.SelectedValue = ds.Tables[0].Rows[0]["FLDACCESSLEVEL"].ToString();

                ucCompany.SelectedCompany = ds.Tables[0].Rows[0]["FLDCOMPANYID"].ToString();

                txtnoofcolumns.Text = ds.Tables[0].Rows[0]["FLDNOOFCOLUMNS"].ToString();
                ViewState["NOOFCOLUMNS"] = ds.Tables[0].Rows[0]["FLDNOOFCOLUMNS"].ToString();
                ViewState["LOCK"] = ds.Tables[0].Rows[0]["FLDISLOCK"].ToString();
            }

            //PhoenixToolbar toolbar = new PhoenixToolbar();
            ////toolbar.AddImageButton("../Inspection/InspectionCDISIREMatrixCategory.aspx", "Add", "add.png", "ADD");
            ////toolbar.AddImageLink("javascript:Openpopup('Filter','','../Inspection/InspectionCDISIREMatrixContentAdd.aspx?categoryid='" + General.GetNullableGuid(lblSelectedNode.Text) + "&noofcolumns=" + txtnoofcolumns.Text + "'); return false;", "Add", "add.png", "ADD");
            //toolbar.AddImageLink("javascript:parent.Openpopup('Filter','','../Inspection/InspectionCDISIREMatrixContentAdd.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&noofcolumns=" + General.GetNullableInteger(ViewState["NOOFCOLUMNS"].ToString()) + "'); return false;", "Add", "add.png", "ADD");
            ////toolbar.AddImageLink("javascript:Openpopup('codehelp1','','InspectionIncidentAdd.aspx');return true;", "Add New", "add.png", "ADD");
            //toolbar.AddImageButton("../Common/CommonPhoenixQuery.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
            //MenuPhoenixQuery.MenuList = toolbar.Show();
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
        gvCDISIREMatrix.Visible = false;
    }

    protected void ddlRefNo_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        gvCDISIREMatrix.Visible = true;
        DocumentCategoryEdit();
        CreateDynamicTextBox();
        if (Request.QueryString["inspectionid"].ToString() != null && ddlRefNo.SelectedValue != null)
        {
            BindData();
            MenuPhoenixQuery.Visible = true;
        }
        //configpart.Visible = false;
    }


    protected void ddlCompany_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        gvCDISIREMatrix.Visible = true;
        DocumentCategoryEdit();
        CreateDynamicTextBox();
        BindData();
        //configpart.Visible = false;
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