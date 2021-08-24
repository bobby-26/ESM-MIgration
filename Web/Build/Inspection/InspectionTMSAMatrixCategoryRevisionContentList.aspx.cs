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

public partial class Inspection_InspectionTMSAMatrixCategoryRevisionContentList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        lblSelectedNode.Visible = false;
        lblCategoryId.Visible = false;
        configpart.Visible = false;
        navigationPane.Visible = false;
        tvwDocumentCategory.Visible = false;
        RadSplitbar1.Visible = false;


        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionTMSAMatrixCategoryRevisionContentList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
        MenuPhoenixQuery.MenuList = toolbar.Show();
        MenuPhoenixQuery.Visible = true;

        PhoenixToolbar toolbarmain = new PhoenixToolbar();

        MenuDocumentCategoryMain.AccessRights = this.ViewState;
        MenuDocumentCategoryMain.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            confirmReview.Attributes.Add("style", "display:none;");

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["CATEGORYID"] = null;


            if (Request.QueryString["categoryid"] != null)
                lblSelectedNode.Text = Request.QueryString["categoryid"].ToString();

            if (Request.QueryString["revisionid"] != null)
                ViewState["REVISIONID"] = Request.QueryString["revisionid"].ToString();
            else
                ViewState["REVISIONID"] = null;

            if (Request.QueryString["noofcolumns"] != null)
                txtnoofcolumns.Text = Request.QueryString["noofcolumns"].ToString();


            ucCompany.Enabled = false;
            ucCompany.SelectedCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString();
            BindDocumentCategoryTree();
            DocumentCategoryEdit();
            CreateDynamicTextBox();
            //MenuPhoenixQuery.Visible = false;

            if (Request.QueryString["categoryid"] != null && ViewState["REVISIONID"] != null)
            {
                BindData();
                MenuPhoenixQuery.Visible = true;
            }
        }

    }

    protected void MenuPhoenixQuery_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("ADD"))
        {

            Response.Redirect("../Inspection/InspectionTMSAMatrixContentAdd.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&noofcolumns=" + General.GetNullableInteger(ViewState["NOOFCOLUMNS"].ToString()) + "&action=add");

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

        if (CommandName.ToUpper().Equals("REVISIONS"))
        {
            Response.Redirect("../Inspection/InspectionTMSAMatrixCategoryRevisions.aspx?CATEGORYID=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()));
        }
    }

    protected void ShowExcel()
    {

        DataTable dt = new DataTable();
        dt = PhoenixInspectionTMSAMatrix.EditTMSAMatrixColumnHeaders(General.GetNullableGuid(lblSelectedNode.Text));

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

        columns = columns + ",FLDPROCEDURES,FLDOBJECTIVEEVIDENCE,FLDDEPARTMENTNAMELIST,FLDCOMPLY";
        captions = captions + ",Procedures, Objective Evidence,Responsibility,Comply";

        string[] alColumns = columns.Split(',');
        string[] alCaptions = captions.Split(',');

        DataTable dtcolumn = new DataTable();

        dtcolumn = PhoenixInspectionTMSAMatrix.TMSAMatrixCategoryContentRevisionSearch(General.GetNullableGuid(lblSelectedNode.Text), General.GetNullableGuid(ViewState["REVISIONID"].ToString()));

        if (dtcolumn.Rows.Count > 0)
        {
            ShowQuery(General.GetNullableGuid(lblSelectedNode.Text), dt, dtcolumn);
        }
        else
        {
            gvTMSAMatrix.Visible = false;

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
            gvTMSAMatrix.Visible = false;
        }
        else
        {
            gvTMSAMatrix.Visible = true;
            lblSelectedNode.Text = tvsne.Node.Value.ToString();
            lblCategoryId.Text = tvsne.Node.Text.ToString();

            DocumentCategoryEdit();
            CreateDynamicTextBox();
            BindData();
            configpart.Visible = false;
        }
    }


    protected void gvTMSAMatrix_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
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


            DataTable dt = new DataTable();
            dt = PhoenixInspectionTMSAMatrix.EditTMSAMatrixColumnHeaders(General.GetNullableGuid(lblSelectedNode.Text));

            DataTable dtcolumn = new DataTable();

            dtcolumn = PhoenixInspectionTMSAMatrix.TMSAMatrixCategoryContentRevisionSearch(General.GetNullableGuid(lblSelectedNode.Text), General.GetNullableGuid(ViewState["REVISIONID"].ToString()));

            if (dtcolumn.Rows.Count > 0)
            {
                ShowQuery(General.GetNullableGuid(lblSelectedNode.Text), dt, dtcolumn);
            }
            else
            {
                gvTMSAMatrix.Visible = false;
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

        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            GridDataItem item = (GridDataItem)e.Item;

            //string lblid = ((RadLabel)_gridView.Items[nCurrentRow].FindControl("lblid")).Text;

            Guid lblid = new Guid(item.GetDataKeyValue("FLDCONTENTID").ToString());



            Response.Redirect("../Inspection/InspectionTMSAMatrixContentAdd.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&noofcolumns=" + General.GetNullableInteger(ViewState["NOOFCOLUMNS"].ToString())
                                    + "&contentid=" + lblid + "&action=edit");
        }

        if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            Guid lblid = new Guid(item.GetDataKeyValue("FLDCONTENTID").ToString());
            PhoenixInspectionTMSAMatrix.DeleteTMSAMatrixContent(lblid);
            ucStatus.Text = "Document is deleted.";
            BindData();
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
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            if (del != null)
            {
                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
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
                    + PhoenixModule.QUALITY + "&type=SHIPBOARDEVIDENCE&cmdname=SHIPBOARDEVIDENCEUPLOAD&VESSELID=0" + "'); return false;");
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
                    + PhoenixModule.QUALITY + "&type=BPGEXAMPLE&cmdname=SHIPBOARDEVIDENCEUPLOAD&VESSELID=0" + "'); return false;");
            }

            if (drv["FLDFORMPOSTERCHECKLISTS"].ToString() != "" || drv["FLDPROCEDURES"].ToString() != "")
            {

                DataSet dss = PhoenixInspectionTMSAMatrix.FormPosterRevisionList(ViewState["CONTENTID"] == null ? null : General.GetNullableGuid(ViewState["CONTENTID"].ToString()),
                                                        General.GetNullableGuid(ViewState["CATEGORYID"].ToString()), 0, General.GetNullableGuid(ViewState["REVISIONID"].ToString()));
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
                            hl.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentGeneral.aspx?showall=0&DOCUMENTID=" + dr["FLDFORMPOSTERID"].ToString() + "');return false;");
                        else if (dr["FLDTYPE"].ToString() == "3")
                            hl.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionContentView.aspx?callfrom=documenttree&SECTIONID=" + dr["FLDFORMPOSTERID"].ToString() + "');return false;");
                        else if (dr["FLDTYPE"].ToString() == "5")
                        {
                            hl.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementFormPreview.aspx?FORMID=" + dr["FLDFORMPOSTERID"].ToString() + "&FORMREVISIONID=" + "');return false;");
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
                    DataSet dsreports = PhoenixInspectionTMSAMatrix.FormReportRevisionList(ViewState["CONTENTID"] == null ? null : General.GetNullableGuid(ViewState["CONTENTID"].ToString()),
                                                        General.GetNullableGuid(ViewState["CATEGORYID"].ToString()), General.GetNullableGuid(ViewState["REVISIONID"].ToString()));
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
            for (int i = noofcolumns + 6; i < gvTMSAMatrix.Columns.Count; i++)
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
                    TemplateField field = new TemplateField();
                    columnheader = "FLDCOLUMN" + i.ToString();
                    columnheadervalue = "FLD" + dr[columnheader].ToString().Replace(" ", "").ToUpper();
                    field.HeaderTemplate = new GridViewTemplate(ListItemType.Header, dr[columnheader].ToString());
                    gvTMSAMatrix.Columns.Add(field);
                }
            }
        }
        return dt.Rows.Count;
    }

    protected void confirmReview_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixInspectionTMSAMatrix.PublishTMSAMatrixCategory(
                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                        , General.GetNullableGuid(lblSelectedNode.Text)
                                        , General.GetNullableDateTime(DateTime.Now.ToString())
                                        );

            //Filter.CurrentSelectedComments = null;
            ucStatus.Text = "Document published.";
            BindData();

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


        public GridViewTemplate(ListItemType type, string columnname)
        {
            _templatetype = type;
            _columnname = columnname;
            _fieldtype = "Label";
        }

        public GridViewTemplate(ListItemType type)
        {
            _templatetype = type;
            _columnname = "Test";
            _fieldtype = "TextBox";
        }

        public GridViewTemplate(ListItemType type, string columnname, string fieldtype)
        {
            _templatetype = type;
            _columnname = columnname;
            _fieldtype = fieldtype;
        }

        //protected void gvTMSAMatrix_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
                case "TEXTBOX":
                    TextBox txt = (TextBox)sender;
                    txt.Text = value.ToString();
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
                    Literal ltl1 = new Literal();
                    ltl1.Text = _columnname;
                    container.Controls.Add(ltl1);
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

                txtnoofcolumns.Text = ds.Tables[0].Rows[0]["FLDNOOFCOLUMNS"].ToString();
                ViewState["NOOFCOLUMNS"] = ds.Tables[0].Rows[0]["FLDNOOFCOLUMNS"].ToString();
            }

            //PhoenixToolbar toolbar = new PhoenixToolbar();
            ////toolbar.AddImageButton("../Inspection/InspectionTMSAMatrixCategory.aspx", "Add", "add.png", "ADD");
            ////toolbar.AddImageLink("javascript:Openpopup('Filter','','../Inspection/InspectionTMSAMatrixContentAdd.aspx?categoryid='" + General.GetNullableGuid(lblSelectedNode.Text) + "&noofcolumns=" + txtnoofcolumns.Text + "'); return false;", "Add", "add.png", "ADD");
            //toolbar.AddImageLink("javascript:parent.Openpopup('Filter','','../Inspection/InspectionTMSAMatrixContentAdd.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&noofcolumns=" + General.GetNullableInteger(ViewState["NOOFCOLUMNS"].ToString()) + "'); return false;", "Add", "add.png", "ADD");
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
}