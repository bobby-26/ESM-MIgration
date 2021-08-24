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
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class Inspection_InspectionTMSAMatrixContentAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbarmain.AddButton("Back", "BACK", ToolBarDirection.Right);

        MenuDocumentCategoryMain.AccessRights = this.ViewState;
        MenuDocumentCategoryMain.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["CATEGORYID"] = null;
            ViewState["NOOFCOLUMNS"] = null;
            ViewState["FLDCONTENTID"] = null;
            ViewState["INSPECTIONID"] = null;
            ViewState["NOOFSTAGES"] = null;

            DataSet ds = PhoenixRegistersDepartment.Listdepartment(1, 2, null);
            DataTable dt = ds.Tables[0];
            chkDeptList.DataSource = dt;
            chkDeptList.DataBind();

            if (Request.QueryString["categoryid"] != null)
                ViewState["CATEGORYID"] = Request.QueryString["categoryid"].ToString();

            if (Request.QueryString["noofcolumns"] != null)
                ViewState["NOOFCOLUMNS"] = Request.QueryString["noofcolumns"].ToString();

            if (Request.QueryString["noofstages"] != null)
                ViewState["NOOFSTAGES"] = Request.QueryString["noofstages"].ToString();

            if (Request.QueryString["contentid"] != null)
                ViewState["FLDCONTENTID"] = Request.QueryString["contentid"].ToString();
            else
                ViewState["FLDCONTENTID"] = null;

            if (Request.QueryString["inspectionid"] != null)
                ViewState["INSPECTIONID"] = Request.QueryString["inspectionid"].ToString();
            else
                ViewState["INSPECTIONID"] = null;

            if (Request.QueryString["action"] != null)
                ViewState["ACTION"] = Request.QueryString["action"].ToString();
            else
                ViewState["ACTION"] = "";

            DefaultColumnEdit();

        }

        int companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
        btnShowDocuments.Attributes.Add("onclick", "return showPickList('spnPickListDocument', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTree.aspx?iframignore=true&companyid=" + companyid + "'); ");
        btnShowFMS.Attributes.Add("onclick", "return showPickList('spnPickListFMS', 'codehelp1', '', '../Common/CommonPickListFMS.aspx?iframignore=true&companyid=" + companyid + "'); ");

        CreateDynamicTextBox();
        BindFormPosters();
        BindFormReports();

    }


    protected void DefaultColumnEdit()
    {
        if (ViewState["FLDCONTENTID"] != null)
        {
            DataTable dt = new DataTable();
            dt = PhoenixInspectionTMSAMatrix.EditTMSAMatrixContent(General.GetNullableGuid(ViewState["CATEGORYID"].ToString()),
                    General.GetNullableGuid(ViewState["FLDCONTENTID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                txtObj.Text = dt.Rows[0]["FLDOBJECTIVEEVIDENCE"].ToString();
                txtDeptlist.Text = dt.Rows[0]["FLDDEPARTMENTNAMELIST"].ToString();
                if (chkDeptList != null)
                {
                    foreach (ListItem item in chkDeptList.Items)
                    {
                        item.Selected = false;
                        if (!string.IsNullOrEmpty(dt.Rows[0]["FLDDEPARTMENTLIST"].ToString()) && ("," + dt.Rows[0]["FLDDEPARTMENTLIST"].ToString() + ",").Contains("," + item.Value.ToString() + ","))
                            item.Selected = true;
                    }
                }
            }
        }
    }


    protected void MenuDocumentCategoryMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        Guid? contentid = null;

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            string strDeptlist = "";

            foreach (ListItem item in chkDeptList.Items)
            {
                if (item.Selected)
                {
                    strDeptlist = strDeptlist + item.Value + ",";
                }
            }

            if (ViewState["CATEGORYID"] != null && ViewState["FLDCONTENTID"] == null && ViewState["ACTION"].ToString() == "add")
            {
                //insert

                PhoenixInspectionTMSAMatrix.InsertTMSAMatrixCategoryContent(
                                                     null,
                                                     General.GetNullableGuid(ViewState["CATEGORYID"].ToString()),
                                                     General.GetNullableString(txtDocumentName.Text),
                                                     General.GetNullableString(strDeptlist),
                                                     General.GetNullableString(txtObj.Text), null, null, ref contentid, General.GetNullableGuid(ViewState["INSPECTIONID"].ToString())
                                                     );                //PhoenixInspectionTMSAMatrix.InsertTMSAMatrixColumnHeaders(General.GetNullableGuid(lblSelectedNode.Text), General.GetNullableInteger(txtnoofcolumns.Text),
                                                                       //    General.GetNullableInteger(ucCompany.SelectedCompany), null, null);

                ViewState["FLDCONTENTID"] = contentid.ToString();
                int n = int.Parse(ViewState["NOOFCOLUMNS"].ToString());
                if (n > 0)
                {
                    //string column = "";
                    string columnvalue = "";
                    for (int i = 1; i <= n; i++)
                    {
                        RadTextBox MyTextBox = new RadTextBox();
                        if (MyTextBox != null)
                        {
                            string columnheader = "";
                            string ss = "";
                            if (i == 1)
                            {
                                ss = (Request.Form["RadComboBox1"].ToString());
                            }
                            else
                            {
                                ss = (Request.Form["txtbox" + (i).ToString()].ToString()).Replace("'", "''");

                            }
                            columnheader = "COLUMN" + i.ToString();
                            DataTable ds = new DataTable();
                            ds = PhoenixInspectionTMSAMatrix.EditTMSAMatrixColumnHeaders(General.GetNullableGuid(ViewState["CATEGORYID"].ToString()), General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()));
                            //column = ds.Rows[0][columnheader].ToString();
                            columnvalue = ss;
                            if (MyTextBox != null)
                            {
                                PhoenixInspectionTMSAMatrix.UpdateTMSAMatrixCategoryContent(
                                                                     General.GetNullableGuid(contentid.ToString()),
                                                                     General.GetNullableGuid(ViewState["CATEGORYID"].ToString()),
                                                                     General.GetNullableString(strDeptlist),
                                                                     General.GetNullableString(txtObj.Text),
                                                                     General.GetNullableString(columnheader),
                                                                     General.GetNullableString(columnvalue)
                                                                     );

                            }

                        }
                    }
                }
            }
            if (ViewState["CATEGORYID"] != null && ViewState["FLDCONTENTID"] != null)
            {
                int count = 1;
                if(ViewState["ACTION"].ToString() == "edit")
                {
                    count = 2;
                }
                int n = int.Parse(ViewState["NOOFCOLUMNS"].ToString());
                if (n > 0)
                {
                    // string column = "";
                    string columnvalue = "";
                    for (int i = count; i <= n; i++)
                    {
                        RadTextBox MyTextBox = new RadTextBox();
                        if (MyTextBox != null)
                        {
                            string columnheader = "";
                            string ss = "";
                            if (i == 1)
                            {
                                ss = (Request.Form["RadComboBox1"].ToString());
                            }
                            else
                            {
                                ss = (Request.Form["txtbox" + (i).ToString()].ToString()).Replace("'", "''");

                            }
                            columnheader = "COLUMN" + i.ToString();
                            DataTable ds = new DataTable();
                            ds = PhoenixInspectionTMSAMatrix.EditTMSAMatrixColumnHeaders(General.GetNullableGuid(ViewState["CATEGORYID"].ToString()), General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()));
                            //column = ds.Rows[0][columnheader].ToString();
                            columnvalue = ss;
                            if (MyTextBox != null)
                            {
                                PhoenixInspectionTMSAMatrix.UpdateTMSAMatrixCategoryContent(
                                                                     General.GetNullableGuid(ViewState["FLDCONTENTID"].ToString()),
                                                                     General.GetNullableGuid(ViewState["CATEGORYID"].ToString()),
                                                                     General.GetNullableString(strDeptlist),
                                                                     General.GetNullableString(txtObj.Text),
                                                                     General.GetNullableString(columnheader),
                                                                     General.GetNullableString(columnvalue)
                                                                     );
                            }

                        }
                    }
                    PhoenixInspectionTMSAMatrix.UpdateTMSAMatrixCategoryDefaultContent(
                                                         General.GetNullableGuid(ViewState["FLDCONTENTID"].ToString()),
                                                         General.GetNullableGuid(txtDocumentId.Text),
                                                         General.GetNullableGuid(ViewState["CATEGORYID"].ToString()),
                                                         General.GetNullableString(txtDocumentName.Text),
                                                         General.GetNullableString(txtObj.Text)
                                                         );


                }
            }
            DefaultColumnEdit();

        }
        if (CommandName.ToUpper().Equals("BACK"))
        {
            Response.Redirect("../Inspection/InspectionTMSAMatrixCategoryContentList.aspx?categoryid=" + General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) + "&noofcolumns=" + General.GetNullableInteger(ViewState["NOOFCOLUMNS"].ToString()) + "");
        }
    }

    public void rblType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindFormPosters();
    }

    protected void BindFormPosters()
    {
        DataSet dss = PhoenixInspectionTMSAMatrix.FormPosterList(ViewState["FLDCONTENTID"] == null ? null : General.GetNullableGuid(ViewState["FLDCONTENTID"].ToString()),
                                                General.GetNullableGuid(ViewState["CATEGORYID"].ToString()), General.GetNullableInteger(rblType.SelectedValue));
        int number = 1;
        if (dss.Tables[0].Rows.Count > 0 && dss.Tables[0].Columns.Count > 1)
        {
            tblForms.Rows.Clear();
            foreach (DataRow dr in dss.Tables[0].Rows)
            {
                CheckBox cb = new CheckBox();
                cb.ID = dr["FLDFORMPOSTERID"].ToString();
                cb.Text = "";
                cb.Checked = true;
                cb.AutoPostBack = true;
                if (ViewState["status"] != null && ViewState["status"].ToString().Equals("3"))
                    cb.Enabled = false;
                cb.CheckedChanged += new EventHandler(cb_CheckedChanged);
                //cb.Attributes.Add("onclick","");
                HyperLink hl = new HyperLink();
                hl.Text = dr["FLDNAME"].ToString();
                hl.ID = "hlink" + number.ToString();
                hl.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");

                int type = 0;
                PhoenixIntegrationQuality.GetSelectedeTreeNodeType(General.GetNullableGuid(dr["FLDFORMPOSTERID"].ToString()), ref type);
                if (type == 2)
                    hl.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '../DocumentManagement/DocumentManagementDocumentGeneral.aspx?showall=0&DOCUMENTID=" + dr["FLDFORMPOSTERID"].ToString() + "');return false;");
                else if (type == 3)
                    hl.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '../DocumentManagement/DocumentManagementDocumentSectionContentView.aspx?callfrom=documenttree&SECTIONID=" + dr["FLDFORMPOSTERID"].ToString() + "');return false;");
                else if (type == 5)
                {
                    DataSet ds = PhoenixIntegrationQuality.FormEdit(
                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                            , new Guid(dr["FLDFORMPOSTERID"].ToString()));

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
                tc.Controls.Add(cb);
                tr.Cells.Add(tc);
                tc = new HtmlTableCell();
                tc.Controls.Add(hl);
                tr.Cells.Add(tc);
                tblForms.Rows.Add(tr);
                tc = new HtmlTableCell();
                tc.InnerHtml = "<br/>";
                tr.Cells.Add(tc);
                tblForms.Rows.Add(tr);
                number = number + 1;
            }
            divForms.Visible = true;
        }
        else
            divForms.Visible = false;
    }
    protected void BindFormReports()
    {
        DataSet dss = PhoenixInspectionTMSAMatrix.FormReportList(ViewState["FLDCONTENTID"] == null ? null : General.GetNullableGuid(ViewState["FLDCONTENTID"].ToString()),
                                                General.GetNullableGuid(ViewState["CATEGORYID"].ToString()));
        int number = 1;
        if (dss.Tables[0].Rows.Count > 0 && dss.Tables[0].Columns.Count > 1)
        {
            tblReports.Rows.Clear();
            foreach (DataRow dr in dss.Tables[0].Rows)
            {
                CheckBox cbreport = new CheckBox();
                cbreport.ID = dr["FLDFORMREPORTID"].ToString();
                cbreport.Text = "";
                cbreport.Checked = true;
                cbreport.AutoPostBack = true;
                cbreport.CheckedChanged += new EventHandler(cbreport_CheckedChanged);
                HyperLink hlreport = new HyperLink();
                hlreport.Text = dr["FLDNAME"].ToString();
                hlreport.ID = "reportlink" + number.ToString();
                hlreport.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");
                hlreport.Attributes.Add("onclick", "openNewWindow('codehelp1', '', 'StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&ReportId=" + dr["FLDFORMREPORTID"].ToString() + "');return false;");
                HtmlTableRow tr = new HtmlTableRow();
                HtmlTableCell tc = new HtmlTableCell();
                tc.Controls.Add(cbreport);
                tr.Cells.Add(tc);
                tc = new HtmlTableCell();
                tc.Controls.Add(hlreport);
                tr.Cells.Add(tc);
                tblForms.Rows.Add(tr);
                tc = new HtmlTableCell();
                tc.InnerHtml = "<br/>";
                tr.Cells.Add(tc);
                tblReports.Rows.Add(tr);
                number = number + 1;
            }
            divReports.Visible = true;
        }
        else
            divReports.Visible = false;
    }

    void cbreport_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox c = (CheckBox)sender;
        if (c.Checked == false)
        {
            PhoenixInspectionTMSAMatrix.FormReportDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                new Guid(ViewState["FLDCONTENTID"].ToString()), new Guid(c.ID), General.GetNullableGuid(ViewState["CATEGORYID"].ToString()));

            ucStatus.Text = "Report form deleted.";
            BindFormReports();
        }
    }


    void cb_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox c = (CheckBox)sender;
        if (c.Checked == false)
        {
            PhoenixInspectionTMSAMatrix.FormPosterDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                new Guid(ViewState["FLDCONTENTID"].ToString()), new Guid(c.ID), General.GetNullableGuid(ViewState["CATEGORYID"].ToString()), int.Parse(rblType.SelectedValue));

            string txt = "";
            if (rblType.SelectedValue == "0")
            {
                txt = "Forms/Posters/Checklists";
            }
            else if (rblType.SelectedValue == "1")
            {
                txt = "Procedures";
            }
            else if (rblType.SelectedValue == "2")
            {
                txt = "Contingency/Emergency";
            }

            ucStatus.Text = txt + " deleted.";
            BindFormPosters();
        }
    }


    protected void lnkFormAdd_Click(object sender, EventArgs e)
    {
        Guid? newcontentid = null;

        if (General.GetNullableGuid(txtDocumentId.Text) != null)
        {
            if (rblType.SelectedValue == "0")
            {
                if (ViewState["FLDCONTENTID"] == null)
                {

                    PhoenixInspectionTMSAMatrix.UpdateTMSAMatrixCategoryFormPosterChecklistUpdate(
                                                     ViewState["FLDCONTENTID"] == null ? null : General.GetNullableGuid(ViewState["FLDCONTENTID"].ToString()),
                                                     General.GetNullableGuid(txtDocumentId.Text),
                                                     General.GetNullableGuid(ViewState["CATEGORYID"].ToString()),
                                                     ref newcontentid, General.GetNullableGuid(ViewState["INSPECTIONID"].ToString())
                                                     );
                    ViewState["FLDCONTENTID"] = newcontentid.ToString();
                    ucStatus.Text = "Forms/Posters/Checklists added.";
                    txtDocumentId.Text = "";
                    txtDocumentName.Text = "";
                    BindFormPosters();
                }
                else
                {
                    PhoenixInspectionTMSAMatrix.UpdateTMSAMatrixCategoryFormPosterChecklistUpdate(
                                                     ViewState["FLDCONTENTID"] == null ? null : General.GetNullableGuid(ViewState["FLDCONTENTID"].ToString()),
                                                     General.GetNullableGuid(txtDocumentId.Text),
                                                     General.GetNullableGuid(ViewState["CATEGORYID"].ToString())
                                                     );
                    ucStatus.Text = "Forms/Posters/Checklists added.";
                    txtDocumentId.Text = "";
                    txtDocumentName.Text = "";
                    BindFormPosters();
                }
            }
            else if (rblType.SelectedValue == "1")
            {
                if (ViewState["FLDCONTENTID"] == null)
                {
                    PhoenixInspectionTMSAMatrix.UpdateTMSAMatrixCategoryProceduresUpdate(
                                                     General.GetNullableGuid(ViewState["FLDCONTENTID"].ToString()),
                                                     General.GetNullableGuid(txtDocumentId.Text),
                                                     General.GetNullableGuid(ViewState["CATEGORYID"].ToString()),
                                                     ref newcontentid, General.GetNullableGuid(ViewState["INSPECTIONID"].ToString())
                                                     );
                    ViewState["FLDCONTENTID"] = newcontentid.ToString();
                    ucStatus.Text = "Forms/Posters/Checklists added.";
                    txtDocumentId.Text = "";
                    txtDocumentName.Text = "";
                    BindFormPosters();
                }
                else
                {
                    PhoenixInspectionTMSAMatrix.UpdateTMSAMatrixCategoryProceduresUpdate(
                                                     General.GetNullableGuid(ViewState["FLDCONTENTID"].ToString()),
                                                     General.GetNullableGuid(txtDocumentId.Text),
                                                     General.GetNullableGuid(ViewState["CATEGORYID"].ToString())
                                                     );
                    ucStatus.Text = "Forms/Posters/Checklists added.";
                    txtDocumentId.Text = "";
                    txtDocumentName.Text = "";
                    BindFormPosters();
                }
            }
        }
        else
        {
            if (rblType.SelectedValue == "0")
            {
                ucError.ErrorMessage = "Please select the Forms/Posters/Checklists.";
                ucError.Visible = true;
                return;
            }
            else if (rblType.SelectedValue == "1")
            {
                ucError.ErrorMessage = "Please select the Procedures.";
                ucError.Visible = true;
                return;
            }
        }
    }

    protected void lnkReportAdd_Click(object sender, EventArgs e)
    {
        Guid? newcontentid = null;
        if (General.GetNullableGuid(txtReportId.Text) != null)
        {

            if (ViewState["FLDCONTENTID"] == null)
            {
                PhoenixInspectionTMSAMatrix.UpdateTMSAMatrixCategoryFMSReportsUpdate(
                                                     ViewState["FLDCONTENTID"] == null ? null : General.GetNullableGuid(ViewState["FLDCONTENTID"].ToString()),
                                                     General.GetNullableGuid(txtFormId.Text),
                                                     General.GetNullableGuid(txtReportId.Text),
                                                     General.GetNullableGuid(ViewState["CATEGORYID"].ToString()),
                                                     ref newcontentid,
                                                     General.GetNullableGuid(ViewState["INSPECTIONID"].ToString())
                                                     );

                ViewState["FLDCONTENTID"] = newcontentid.ToString();
                ucStatus.Text = "Reports added.";
                txtReportId.Text = "";
                txtReportName.Text = "";
                BindFormReports();
            }
            else
            {
                PhoenixInspectionTMSAMatrix.UpdateTMSAMatrixCategoryFMSReportsUpdate(
                                                     ViewState["FLDCONTENTID"] == null ? null : General.GetNullableGuid(ViewState["FLDCONTENTID"].ToString()),
                                                     General.GetNullableGuid(txtFormId.Text),
                                                     General.GetNullableGuid(txtReportId.Text),
                                                     General.GetNullableGuid(ViewState["CATEGORYID"].ToString())
                                                     );

                ucStatus.Text = "Reports added.";
                txtReportId.Text = "";
                txtReportName.Text = "";
                BindFormReports();
            }
        }
        else
        {
            ucError.ErrorMessage = "Please select the Reports.";
            ucError.Visible = true;
            return;
        }
    }

    protected void CreateDynamicTextBox()
    {
        if (ViewState["NOOFCOLUMNS"].ToString() != "")
        {
            string columnheader = "";
            string txtbox = "txtbox";
            int n = int.Parse(ViewState["NOOFCOLUMNS"].ToString());
            DataTable ds = new DataTable();
            ds = PhoenixInspectionTMSAMatrix.EditTMSAMatrixColumnHeaders(General.GetNullableGuid(ViewState["CATEGORYID"].ToString()));
            for (int i = 1; i <= n; i++)
            {
                if (i == 1)
                {
                    Label lit4 = new Label();
                    //lit.Height = 25;
                    lit4.Text = "</br>";
                    Pnlcolumnlabel.Controls.Add(lit4);
                    columnheader = "FLDCOLUMNTEXT" + i.ToString();
                    Label lit = new Label();
                    //lit.Height = 25;
                    lit.Text = ds.Rows[0][columnheader].ToString();
                    Pnlcolumnlabel.Controls.Add(lit);
                    Label lit2 = new Label();
                    //lit.Height = 25;
                    lit2.Text = "</br></br>";
                    Pnlcolumnlabel.Controls.Add(lit2);
                    RadTextBox MyTextBox = new RadTextBox();
                    RadComboBox radstage = new RadComboBox();
                    if (ViewState["ACTION"].ToString() == "edit")
                    {
                        //Assigning the textbox ID name  
                        MyTextBox.ID = txtbox + i.ToString();
                        MyTextBox.Width = 300;
                        //MyTextBox.Height = 25;
                        MyTextBox.Font.Name = "Tahoma";
                        MyTextBox.Font.Size = 8;
                        MyTextBox.TextMode = InputMode.SingleLine;
                        //this.Controls.Add(MyTextBox);
                        pnlcolumns.Controls.Add(MyTextBox);
                    }
                    else
                    {
                        radstage.RenderMode = RenderMode.Lightweight;
                        radstage.EmptyMessage = "Type to select";
                        radstage.Filter = RadComboBoxFilter.Contains;
                        radstage.MarkFirstMatch = true;
                        radstage.Width = 100;
                        radstage.ID = "RadComboBox1";
                        radstage.EnableLoadOnDemand = true;
                        radstage.ItemsRequested += new RadComboBoxItemsRequestedEventHandler(radstage_ItemsRequested);
                        pnlcolumns.Controls.Add(radstage);
                    }
                    Label lit1 = new Label();
                    //lit.Height = 25;
                    lit1.Text = "</br></br></br>";
                    pnlcolumns.Controls.Add(lit1);
                    Label lit3 = new Label();
                    //lit.Height = 25;
                    lit3.Text = "</br></br>";
                    Pnlcolumnlabel.Controls.Add(lit3);

                    if (ViewState["FLDCONTENTID"] != null && ViewState["ACTION"].ToString() == "edit")
                    {
                        DataTable dt = new DataTable();
                        dt = PhoenixInspectionTMSAMatrix.EditTMSAMatrixContent(General.GetNullableGuid(ViewState["CATEGORYID"].ToString()),
                                General.GetNullableGuid(ViewState["FLDCONTENTID"].ToString()), General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()));
                        if (dt.Rows.Count > 0)
                        {
                            MyTextBox.Text = dt.Rows[0][i].ToString();
                            MyTextBox.Enabled = false;
                        }
                    }
                }
                else
                {
                    columnheader = "FLDCOLUMNTEXT" + i.ToString();
                    Label lit = new Label();
                    //lit.Height = 25;
                    lit.Text = ds.Rows[0][columnheader].ToString();
                    Pnlcolumnlabel.Controls.Add(lit);
                    Label lit2 = new Label();
                    //lit.Height = 25;
                    lit2.Text = "</br></br></br></br>";
                    Pnlcolumnlabel.Controls.Add(lit2);
                    RadTextBox MyTextBox = new RadTextBox();
                    //Assigning the textbox ID name  
                    MyTextBox.ID = txtbox + i.ToString();
                    MyTextBox.Width = 300;
                    //MyTextBox.Height = 25;
                    MyTextBox.Font.Name = "Tahoma";
                    MyTextBox.Font.Size = 8;
                    MyTextBox.TextMode = InputMode.MultiLine;
                    MyTextBox.Rows = 4;
                    //this.Controls.Add(MyTextBox);
                    pnlcolumns.Controls.Add(MyTextBox);
                    Label lit1 = new Label();
                    //lit.Height = 25;
                    lit1.Text = "</br></br>";
                    pnlcolumns.Controls.Add(lit1);
                    Label lit3 = new Label();
                    //lit.Height = 25;
                    lit3.Text = "</br></br>";
                    Pnlcolumnlabel.Controls.Add(lit3);

                    if (ViewState["FLDCONTENTID"] != null)
                    {
                        DataTable dt = new DataTable();
                        dt = PhoenixInspectionTMSAMatrix.EditTMSAMatrixContent(General.GetNullableGuid(ViewState["CATEGORYID"].ToString()),
                                General.GetNullableGuid(ViewState["FLDCONTENTID"].ToString()), General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()));
                        if (dt.Rows.Count > 0)
                        {
                            MyTextBox.Text = dt.Rows[0][i].ToString();
                        }
                    }
                }
            }
        }
    }

    protected void radstage_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        RadComboBox combo = (RadComboBox)sender;
        int n = int.Parse(ViewState["NOOFSTAGES"].ToString());

        for (int i = 1; i <= n; i++)
        {
            combo.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
        }
    }

}