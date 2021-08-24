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



public partial class Inspection_InspectionCDISIREMatrixCategory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        confirm.Attributes.Add("style", "display:none;");

        SessionUtil.PageAccessRights(this.ViewState);
        lblSelectedNode.Visible = false;
        lblCategoryId.Visible = false;

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionCDISIREMatrixCategory.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        //toolbar.AddImageLink("javascript:parent.Openpopup('Filter','','../Inspection/InspectionCDISIREMatrixContentAdd.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&noofcolumns=" + General.GetNullableInteger(ViewState["NOOFCOLUMNS"].ToString()) + "'); return false;", "Add", "add.png", "ADD");
        //toolbar.AddImageLink("javascript:Openpopup('codehelp1','','InspectionIncidentAdd.aspx');return true;", "Add New", "add.png", "ADD");
        //toolbar.AddFontAwesomeButton("../Common/CommonPhoenixQuery.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
        MenuPhoenixQuery.MenuList = toolbar.Show();
        MenuPhoenixQuery.Visible = true;

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Delete", "DELETE", ToolBarDirection.Right);
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbarmain.AddButton("New", "NEW", ToolBarDirection.Right);

            MenuDocumentCategoryMain.AccessRights = this.ViewState;
            MenuDocumentCategoryMain.MenuList = toolbarmain.Show();
        }

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["CATEGORYID"] = null;
            ViewState["INSPECTIONID"] = null;

            if (Request.QueryString["categoryid"] != null)
                lblSelectedNode.Text = Request.QueryString["categoryid"].ToString();

            if (Request.QueryString["noofcolumns"] != null)
                txtnoofcolumns.Text = Request.QueryString["noofcolumns"].ToString();


            ucCompany.Enabled = false;
            ucCompany.SelectedCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString();
            BindDocumentCategoryTree();
            DocumentCategoryEdit();
            CreateDynamicTextBox();
            MenuPhoenixQuery.Visible = false;

            if (Request.QueryString["categoryid"] != null)
            {
                BindData();
            }
            Bind_UserControls(sender, new EventArgs());

        }
        MenuPhoenixQuery.Visible = false;
        gvCDISIREMatrix.Visible = false;

        //if (Request.QueryString["categoryid"] != null || ViewState["CONTENTID"] != null)
        //{
        //    BindData();
        //}

    }

    protected void Bind_UserControls(object sender, EventArgs e)
    {
        string type = PhoenixCommonRegisters.GetHardCode(1, 148, "INS");
        ucVetting.DataSource = PhoenixInspectionCDISIREMatrix.ListCDISIREInspection(General.GetNullableInteger(type)
                                        , null
                                        , null
                                        , 1
                                        , General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString()));
        ucVetting.DataTextField = "FLDSHORTCODE";
        ucVetting.DataValueField = "FLDINSPECTIONID";
        ucVetting.DataBind();
        ucVetting.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
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
            Response.Redirect("../Inspection/InspectionCDISIREMatrixContentAdd.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&noofcolumns=" + General.GetNullableInteger(ViewState["NOOFCOLUMNS"].ToString()) 
                                                        + "&inspectionid=" + General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()) + "&action=add");

        }

        if (CommandName.ToUpper().Equals("EXCEL"))
        {

        }
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
            tblnoofcolumns.Visible = true;
        }
        else
        {
            tvwDocumentCategory.RootText = "";
            tvwDocumentCategory.PopulateTree(ds.Tables[0]);
            tblnoofcolumns.Visible = true;
        }
        //CreateDynamicTextBox();
        //ViewState["noofcolumns"] = txtnoofcolumns.Text;
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
            ucVetting.SelectedValue = "Dummy";
            MenuPhoenixQuery.Visible = false;
            gvCDISIREMatrix.Visible = false;
        }
        else
        {
            gvCDISIREMatrix.Visible = true;
            lblSelectedNode.Text = tvsne.Node.Value.ToString();
            lblCategoryId.Text = tvsne.Node.Text.ToString();

            DocumentCategoryEdit();
            CreateDynamicTextBox();
            //BindData();
            MenuPhoenixQuery.Visible = false;
            gvCDISIREMatrix.Visible = false;
            //txtnoofcolumns.Enabled = false;
            DataSet ds = new DataSet();
            if (!string.IsNullOrEmpty(lblSelectedNode.Text))
            {
                ds = PhoenixInspectionCDISIREMatrix.CDISIREMatrixCategoryEdit(new Guid(lblSelectedNode.Text));
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
    }

    //protected void rblType_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    BindData();
    //}

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
            dt = PhoenixInspectionCDISIREMatrix.EditCDISIREMatrixColumnHeaders(General.GetNullableGuid(lblSelectedNode.Text), PhoenixSecurityContext.CurrentSecurityContext.VesselID);

            if (dt.Rows[0]["FLDMATRIXTABLENAME"].ToString() != "")
            {
                DataTable dtcolumn = new DataTable();

                dtcolumn = PhoenixInspectionCDISIREMatrix.CDISIREMatrixCategoryContentSearch(General.GetNullableGuid(lblSelectedNode.Text));

                if (dtcolumn.Rows.Count > 0)
                {
                    ShowQuery(General.GetNullableGuid(lblSelectedNode.Text), dt, dtcolumn);
                }
                else
                {
                    gvCDISIREMatrix.Visible = false;
                    //dtcolumn.Rows.Clear();
                    //DataTable dt1 = dtcolumn;
                    //ShowNoRecordsFound(dt1, gvCDISIREMatrix);
                }
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

            Guid lblid = new Guid(item.GetDataKeyValue("FLDID").ToString());

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
            Guid lblid = new Guid(item.GetDataKeyValue("FLDID").ToString());
            //PhoenixDocumentManagementDocument.DocumentDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(lblid));
            ucStatus.Text = "Document is deleted.";
            gvCDISIREMatrix.Rebind();
        }


    }

    protected void gvCDISIREMatrix_ItemDataBound(Object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {

            GridDataItem item = (GridDataItem)e.Item;
            HtmlTable tbl = (HtmlTable)e.Item.FindControl("tblForms");
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



            if (lblid != null)
            {

                DataSet dss = PhoenixInspectionCDISIREMatrix.FormPosterList(ViewState["CONTENTID"] == null ? null : General.GetNullableGuid(ViewState["CONTENTID"].ToString()),
                                                        General.GetNullableGuid(ViewState["CATEGORYID"].ToString()), 0);
                int number = 1;
                if (dss.Tables[0].Rows.Count > 0 && dss.Tables[0].Columns.Count > 1)
                {
                    tbl.Rows.Clear();
                    foreach (DataRow dr in dss.Tables[0].Rows)
                    {
                        CheckBox cb = new CheckBox();
                        cb.ID = dr["FLDFORMPOSTERID"].ToString();
                        cb.Text = "";
                        cb.Checked = true;
                        cb.AutoPostBack = true;
                        cb.Enabled = false;
                        // cb.CheckedChanged += new EventHandler(cb_CheckedChanged);
                        //cb.Attributes.Add("onclick","");
                        HyperLink hl = new HyperLink();
                        hl.Text = dr["FLDNAME"].ToString();
                        hl.ID = "hlink" + number.ToString();
                        hl.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");

                        int type = 0;
                        PhoenixIntegrationQuality.GetSelectedeTreeNodeType(General.GetNullableGuid(dr["FLDFORMPOSTERID"].ToString()), ref type);
                        if (type == 2)
                            hl.Attributes.Add("onclick", "Openpopup('codehelp1', '', '../DocumentManagement/DocumentManagementDocumentGeneral.aspx?showall=0&DOCUMENTID=" + dr["FLDFORMPOSTERID"].ToString() + "');return false;");
                        else if (type == 3)
                            hl.Attributes.Add("onclick", "Openpopup('codehelp1', '', '../DocumentManagement/DocumentManagementDocumentSectionContentView.aspx?callfrom=documenttree&SECTIONID=" + dr["FLDFORMPOSTERID"].ToString() + "');return false;");
                        else if (type == 5)
                        {
                            DataSet ds = PhoenixIntegrationQuality.FormEdit(
                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                    , new Guid(hl.ID.ToString()));

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                DataRow drr = ds.Tables[0].Rows[0];
                                hl.Attributes.Add("onclick", "Openpopup('codehelp1', '', '../DocumentManagement/DocumentManagementFormPreview.aspx?FORMID=" + dr["FLDFORMPOSTERID"].ToString() + "&FORMREVISIONID=" + drr["FLDFORMREVISIONID"].ToString() + "');return false;");
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
            for (int i = noofcolumns + 3; i < gvCDISIREMatrix.Columns.Count; i++)
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
                for (int i = 1; i < noofcolumns; i++)
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
                    TemplateField field = new TemplateField();
                    columnheader = "FLDCOLUMN" + (i).ToString();
                    columnheadertext = "FLDCOLUMNTEXT" + (i).ToString();
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
                    DataSet ds1 = new DataSet();
                    ds1 = PhoenixInspectionCDISIREMatrix.CDISIREMatrixCategoryEdit(new Guid(lblSelectedNode.Text), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        if (string.IsNullOrEmpty(ds1.Tables[0].Rows[0]["FLDPARENTCATEGORYID"].ToString()))
                        {
                            txtnoofcolumns.Enabled = true;
                            MyTextBox.Enabled = true;
                        }
                        else
                        {
                            txtnoofcolumns.Enabled = false;
                            MyTextBox.Enabled = false;
                        }
                    }
                }
            }
            //txtnoofcolumns.Enabled = false;
        }
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
                txtnoofcolumns.Text = "";
                //txtnoofcolumns.Enabled = true;
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
                                                         General.GetNullableInteger(txtnoofcolumns.Text),
                                                         PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                                         General.GetNullableGuid(ucVetting.SelectedValue)
                                                         );
                    Reset();
                    ucStatus.Text = "Category is added.";
                    // txtnoofcolumns.Enabled = false;

                    //PhoenixInspectionCDISIREMatrix.InsertCDISIREMatrixColumnHeaders(General.GetNullableGuid(lblSelectedNode.Text), General.GetNullableInteger(txtnoofcolumns.Text),
                    //    General.GetNullableInteger(ucCompany.SelectedCompany), null, null);
                    BindDocumentCategoryTree();
                    CreateDynamicTextBox();
                    //RadWindowManager1.RadConfirm("Once you defined the no of columns for the section, you cannot add/remove the columns furthermore", "confirm", 320, 150, null, "Confirm");

                }
                else
                {
                    DataSet dt = new DataSet();
                    dt = PhoenixInspectionCDISIREMatrix.CDISIREMatrixCategoryEdit(new Guid(lblSelectedNode.Text),PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                    ViewState["noofcolumns"] = dt.Tables[0].Rows[0]["FLDNOOFCOLUMNS"].ToString();
                    //update
                    PhoenixInspectionCDISIREMatrix.UpdateCDISIREMatrixCategory(
                                                        new Guid(lblSelectedNode.Text),
                                                        txtDocumentCategory.Text.Trim(),
                                                        chkActiveyn.Checked == true ? 1 : 0,
                                                        txtCategoryNumber.Text,
                                                        General.GetNullableInteger(ucCompany.SelectedCompany),
                                                        General.GetNullableInteger(txtnoofcolumns.Text),
                                                        PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                                        new Guid(ucVetting.SelectedValue)
                                                        );

                    if (txtnoofcolumns.Text != "")
                    {
                        int n = int.Parse(txtnoofcolumns.Text);
                        if (n > 0)
                        {
                            string column = "";
                            string columnvalue = "";
                            for (int i = 1; i <= n; i++)
                            {
                                TextBox MyTextBox = new TextBox();
                                if ((ViewState["noofcolumns"].ToString() == "" || int.Parse(ViewState["noofcolumns"].ToString()) >= n) && string.IsNullOrEmpty(dt.Tables[0].Rows[0]["FLDPARENTCATEGORYID"].ToString()))
                                {
                                    string ss = Request.Form["txtbox" + (i).ToString()].ToString();
                                    column = "column" + i.ToString();
                                    columnvalue = ss;
                                    if (MyTextBox != null)
                                    {
                                        PhoenixInspectionCDISIREMatrix.InsertCDISIREMatrixColumnHeaders(new Guid(lblSelectedNode.Text), General.GetNullableInteger(txtnoofcolumns.Text),
                                            General.GetNullableInteger(ucCompany.SelectedCompany), General.GetNullableString(column), General.GetNullableString(columnvalue), PhoenixSecurityContext.CurrentSecurityContext.VesselID);

                                    }
                                }
                            }



                        }
                    }
                    ucStatus.Text = "Category updated.";
                    BindData();
                    CreateDynamicTextBox();
                    BindDocumentCategoryTree();
                }

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

    protected void confirm_Click(object sender, EventArgs e)
    {
        try
        {
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
            txtnoofcolumns.Enabled = false;

            //PhoenixInspectionCDISIREMatrix.InsertCDISIREMatrixColumnHeaders(General.GetNullableGuid(lblSelectedNode.Text), General.GetNullableInteger(txtnoofcolumns.Text),
            //    General.GetNullableInteger(ucCompany.SelectedCompany), null, null);
            BindDocumentCategoryTree();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
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

                txtDocumentCategory.Text = ds.Tables[0].Rows[0]["FLDCATEGORYNAME"].ToString();
                chkActiveyn.Checked = ds.Tables[0].Rows[0]["FLDACTIVEYN"].ToString() == "1" ? true : false;
                lblCategoryId.Text = ds.Tables[0].Rows[0]["FLDCATEGORYID"].ToString();
                txtCategoryNumber.Text = ds.Tables[0].Rows[0]["FLDCATEGORYNUMBER"].ToString();
                //rListAdd.SelectedValue = ds.Tables[0].Rows[0]["FLDACCESSLEVEL"].ToString();

                ucCompany.SelectedCompany = ds.Tables[0].Rows[0]["FLDCOMPANYID"].ToString();

                txtnoofcolumns.Text = ds.Tables[0].Rows[0]["FLDNOOFCOLUMNS"].ToString();
                ViewState["NOOFCOLUMNS"] = ds.Tables[0].Rows[0]["FLDNOOFCOLUMNS"].ToString();
                txtcategoryshortcode.Text = ds.Tables[0].Rows[0]["FLDCATEGORYSHORDCODE"].ToString();
                ucVetting.SelectedValue = ds.Tables[0].Rows[0]["FLDINSPECTIONID"].ToString();
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
        txtcategoryshortcode.Text = "";
        txtnoofcolumns.Text = "";
        ucVetting.SelectedValue = "Dummy";
        //txtnoofcolumns.Enabled = true;
        DataSet ds = new DataSet();
        if (!string.IsNullOrEmpty(lblSelectedNode.Text))
        {
            ds = PhoenixInspectionCDISIREMatrix.CDISIREMatrixCategoryEdit(new Guid(lblSelectedNode.Text),PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ucVetting.SelectedValue = ds.Tables[0].Rows[0]["FLDINSPECTIONID"].ToString();
                if (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["FLDPARENTCATEGORYID"].ToString()))
                {
                    tblnoofcolumns.Visible = false;
                }
                else
                    tblnoofcolumns.Visible = true;
            }
        }
    }

}