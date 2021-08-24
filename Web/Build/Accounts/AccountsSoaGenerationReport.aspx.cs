using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using iTextSharp.text;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web;
using Telerik.Web.UI;
using PdfSharp.Drawing;
using System.IO;

public partial class AccountsSoaGenerationReport : PhoenixBasePage
{
    string cmdname = string.Empty;
    string attachmentcode = string.Empty;
    PhoenixModule module = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("SOA", "SOA");
            toolbarmain.AddButton("Line Items", "LINEITEMS");
            toolbarmain.AddButton("History", "HISTORY");
            MenuGeneral.AccessRights = this.ViewState;
            MenuGeneral.MenuList = toolbarmain.Show();
            MenuGeneral.SelectedMenuIndex = 1;

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Details", "DETAILS");
            toolbarsub.AddButton("Report", "REPORT");
            //toolbarsub.AddButton("Voucher", "VOUCHER");

            MenuGenralSub.AccessRights = this.ViewState;
            MenuGenralSub.MenuList = toolbarsub.Show();
            MenuGenralSub.SelectedMenuIndex = 1;

            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            confirm.Attributes.Add("style", "display:none;");
            confirmucConfirmSOAScheduleMsg.Attributes.Add("style", "display:none;");
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["RECORDNUMBER"] = General.ShowRecords(null);

                if (General.GetNullableGuid(Request.QueryString["debitnoteid"].ToString()) != null)
                    ViewState["debitnotereferenceid"] = Request.QueryString["debitnoteid"].ToString();
                else
                    ViewState["debitnotereferenceid"] = "";
            }

            BindSOADetails();
            StatusbasedVisibilty();

            if (ViewState["DTKey"] != null)
                attachmentcode = ViewState["DTKey"].ToString();

            // BindData();
            BindOwnerData();
            // BindCombinedPDF();
            BindManualAndAdditional();
            //BindManualReportData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void MenuGenralSub_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("VOUCHER"))
            {
                Response.Redirect("../Accounts/AccountsSOAGenerationLineitemupdate.aspx?debitnotereference=" + ViewState["debitnotereference"] + "&accountid=" + ViewState["accountid"] + "&Ownerid=" + ViewState["Ownerid"] + "&voucherdetailid=" + ViewState["FLDVOUCHERDETAILID"] + "&VesselId=" + null + "&debitnoteid=" + ViewState["debitnotereferenceid"] + "&SOAStatus=" + ViewState["SOAStatus"], true);
            }
            else if (CommandName.ToUpper().Equals("REPORT"))
            {
                Response.Redirect("../Accounts/AccountsSoaGenerationReport.aspx?debitnotereference=" + ViewState["debitnotereference"] + "&accountid=" + ViewState["accountid"] + "&Ownerid=" + ViewState["Ownerid"] + "&voucherdetailid=" + ViewState["FLDVOUCHERDETAILID"] + "&VesselId=" + null + "&debitnoteid=" + ViewState["debitnotereferenceid"] + "&SOAStatus=" + ViewState["SOAStatus"], true);
            }
            if (CommandName.ToUpper().Equals("DETAILS"))
            {
                Response.Redirect("../Accounts/AccountsSoaGenerationLineItems.aspx?debitnotereference=" + ViewState["debitnotereference"].ToString() + "&accountid=" + ViewState["accountid"] + "&Ownerid=" + ViewState["Ownerid"] + "&debitnoteid=" + ViewState["debitnotereferenceid"], true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SOA"))
            {
                Response.Redirect("../Accounts/AccountsSOAGeneration.aspx", true);
            }
            if (CommandName.ToUpper().Equals("HISTORY"))
            {
                //Response.Redirect("../Accounts/AccountsSOAGenerationPdfAttachment.aspx?debitnotereferenceid=" + ViewState["debitnotereferenceid"], true);
                Response.Redirect("../Accounts/AccountsDebitNoteReferenceHistory.aspx?DebitNoteReferenceid=" + ViewState["debitnotereferenceid"] + "&qfrom=SOAGeneration");

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuSOALineItems_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("COMBINEDPDF"))
            {
                string scriptpopup = String.Format(
                "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsSOAGenerationSubledgerType.aspx?pdfGen=1&debitnotereference=" + ViewState["debitnotereference"].ToString() + "&debitnotereferenceid=" + ViewState["debitnotereferenceid"].ToString() + "&accountid="
                                 + ViewState["accountid"].ToString() + "&Ownerid=" + ViewState["Ownerid"].ToString() + "&dtkey=" + ViewState["DTKey"].ToString() + "&description=" + null + "&year=" + ViewState["year"].ToString() + "&month=" + ViewState["month"].ToString() + "&URL=" + ViewState["URL"].ToString() + "');");
                if (scriptpopup != "")
                {
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                }

            }
            if (CommandName.ToUpper().Equals("SCHEDULEENTRY"))
            {
                DataSet ds = new DataSet();
                ds = PhoenixAccountsSOAGeneration.SOAGenerationReportList(int.Parse(ViewState["Ownerid"].ToString()), new Guid(ViewState["debitnotereferenceid"].ToString()));
                string Errormsg = "";
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (string.IsNullOrEmpty(ds.Tables[0].Rows[i]["FLDVERIFIEDDETAILS"].ToString()))
                            {
                                Errormsg = Errormsg + ds.Tables[0].Rows[i]["FLDREPORTNAME"].ToString() + "\n";
                            }
                        }
                    }
                }
                if (Errormsg != "")
                {
                    Errormsg = "Following reports to be verified : " + Errormsg;
                    ucError.ErrorMessage = Errormsg;
                    ucError.Visible = true;
                    return;
                }

                ds = null;
                ds = PhoenixAccountsDebitNoteReference.DebitNoteReferenceEdit(new Guid(ViewState["debitnotereferenceid"].ToString()));
                if (ds.Tables.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["FLDCOMBINEDPDFGENDATE"].ToString()))
                    {
                        ucConfirmSOAScheduleMsg.RadConfirm("PDF was already generated earlier and whether they would need a new file to be generated will be displayed. This file will replace previous version of the file", "confirmucConfirmSOAScheduleMsg", 320, 150, null, "Confirm");
                        //ucConfirmSOAScheduleMsg.Visible = true;
                        //ucConfirmSOAScheduleMsg.Text = "PDF was already generated earlier and whether they would need a new file to be generated will be displayed. This file will replace previous version of the file";
                        return;
                    }
                    else
                    {
                        PhoenixAccountsDebitNoteReference.SOAGenerationCombinedPDFScheduleTaskEntry(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["debitnotereferenceid"].ToString()));
                        //Response.Redirect("../Accounts/AccountsSOAGenerationPdfReport.aspx?pdfGen=1&debitnotereference=" + ViewState["debitnotereference"] + "&debitnotereferenceid=" + ViewState["debitnotereferenceid"] + "&accountid="
                        //                   + ViewState["accountid"] + "&Ownerid=" + ViewState["Ownerid"] + "&dtkey=" + ViewState["dtkey"] + "&description=" + ViewState["description"] + "&year=" + ViewState["year"] + "&month=" + ViewState["month"] + "&Type=0&URL=" + ViewState["URL"]);
                    }
                }

            }
            if (CommandName.ToUpper().Equals("ADDLATTACHMNENTS"))
            {
                if (ViewState["AddlDTKey"] != null && ViewState["AddlDTKey"].ToString() != "")
                {
                    ucConfirmAddlreattch.RadConfirm("Owner report already generated.Same will be replaced.Do you want to continue?", "confirmucConfirmAddlreattch", 320, 150, null, "Confirm");
                    //ucConfirmAddlreattch.Visible = true;
                    //ucConfirmAddlreattch.Text = "Owner report already generated.Same will be replaced.Do you want to continue?";
                    return;
                }
                Guid? addlDTKey = BindExcelReports();
                if (addlDTKey != null)
                {
                    PhoenixCommonAcount.UpdateStatementReference(new Guid(ViewState["DTKey"].ToString()));
                    PhoenixAccountsSOAGeneration.SOAGenerationAdditionalAttachmentUpdation(new Guid(ViewState["debitnotereferenceid"].ToString()), addlDTKey);
                    BindManualAndAdditional();
                }

                Response.Redirect("../Accounts/AccountsSoaGenerationReport.aspx?debitnotereference=" + ViewState["debitnotereference"] + "&accountid=" + ViewState["accountid"] + "&Ownerid=" + ViewState["Ownerid"] + "&voucherdetailid=" + ViewState["FLDVOUCHERDETAILID"] + "&VesselId=" + null + "&debitnoteid=" + ViewState["debitnotereferenceid"] + "&SOAStatus=" + ViewState["SOAStatus"], true);
            }
            else if (CommandName.ToUpper().Equals("1STCHECK"))
            {
                if (SOALevelValidation(new Guid(ViewState["debitnotereferenceid"].ToString())) == true)
                {
                    ucConfirmMsg.RadConfirm("Voucher Line items are not verified</br>Open queries are existing</br>Do you want to continue?", "confirm", 320, 150, null, "Confirm");
                    //ucConfirmMsg.Visible = true;
                    //ucConfirmMsg.Text = "Voucher Line items are not verified</br>Open queries are existing</br>Do you want to continue?";
                    return;
                }
                PhoenixAccountsDebitNoteReference.DebitNoteReference1stCheckUpdate(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(ViewState["debitnotereferenceid"].ToString()));
                ucStatus.Text = "Debit Note Reference 1st Level Checked.";
                BindSOADetails();
                StatusbasedVisibilty();
            }
            else if (CommandName.ToUpper().Equals("2NDCHECK"))
            {
                if (SOALevelValidation(new Guid(ViewState["debitnotereferenceid"].ToString())) == true)
                {
                    ucConfirm2ndlevel.RadConfirm("Voucher Line items are not verified</br>Open queries are existing</br>Do you want to continue?", "confirmucConfirm2ndlevel", 320, 150, null, "Confirm");
                    //ucConfirm2ndlevel.Visible = true;
                    //ucConfirm2ndlevel.Text = "Voucher Line items are not verified</br>Open queries are existing</br>Do you want to continue?";
                    return;
                }
                PhoenixAccountsDebitNoteReference.DebitNoteReference2ndCheckUpdate(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(ViewState["debitnotereferenceid"].ToString()));
                ucStatus.Text = "Debit Note Reference 2nd Level Checked.";
                BindSOADetails();
                StatusbasedVisibilty();
            }

            else if (CommandName.ToUpper().Equals("PUBLISH"))
            {
                if (SOALevelValidation(new Guid(ViewState["debitnotereferenceid"].ToString())) == true)
                {
                    ucConfirmpublish.RadConfirm("Voucher Line items are not verified</br>Open queries are existing</br>Do you want to continue?", "confirmucConfirmpublish", 320, 150, null, "Confirm");
                    //ucConfirmpublish.Visible = true;
                    //ucConfirmpublish.Text = "Voucher Line items are not verified</br>Open queries are existing</br>Do you want to continue?";
                    return;
                }
                PhoenixAccountsDebitNoteReference.DebitNoteReferencePublish(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(ViewState["debitnotereferenceid"].ToString()));
                ucStatus.Text = "Debit Note Reference published.";
                BindSOADetails();
                StatusbasedVisibilty();
            }

            else if (CommandName.ToUpper().Equals("UNPUBLISH"))
            {
                PhoenixAccountsDebitNoteReference.DebitNoteReferenceUNPublish(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(ViewState["debitnotereferenceid"].ToString()));

                ucStatus.Text = "Debit Note Reference Unpublished.";
                BindSOADetails();
                StatusbasedVisibilty();
            }

            else if (CommandName.ToUpper().Equals("FINALIZEATTACHMNENTS"))
            {
                FinalizeAttachments();
                ucStatus.Text = "Attachments are updated with Voucher details";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDDEBITNOTEREFERENCE", "FLDTYPE", "FLDACCOUNTCODE", "FLDDESCRIPTION", "FLDNAME", "FLDHARDNAME", "FLDQUICKNAME", "FLDSOASTATUSNAME" };
        string[] alCaptions = { "Statement Reference", "Type", "Account Code", "Account Code Description", "Billing Party", "Year", "Month", "Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = new DataSet();
        ds = PhoenixAccountsSOAGeneration.SOAGenerationReportList(int.Parse(ViewState["Ownerid"].ToString()), new Guid(ViewState["debitnotereferenceid"].ToString()));

        gvOwnersAccount.DataSource = ds;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvOwnersAccount", "Order Form List", alCaptions, alColumns, ds);

    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        try
        {

            ViewState["SORTEXPRESSION"] = ib.CommandName;
            ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }

    private void SetRowSelection()
    {

        for (int i = 0; i < gvOwnersAccount.Items.Count; i++)
        {
            if (gvOwnersAccount.MasterTableView.DataKeyValues[i].ToString().Equals(ViewState["FLDVOUCHERDETAILID"].ToString()))
            {
                gvOwnersAccount.MasterTableView.Items[i].Selected = true;
                ViewState["RVDtKey"] = ((RadLabel)gvOwnersAccount.Items[i].FindControl("lblDtKey")).Text;
                break;
            }
        }
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
        gv.Rows[0].Attributes["onclick"] = "";
    }




    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["FLDVOUCHERDETAILID"] = "";
        BindData();
        BindSOADetails();
    }


    protected void MenuVoucherLI_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper() == "VOUCHERREPORT")
            {

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void BindOwnerData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDDESCRIPTION", "FLDDEBITNOTEREFERENCE", "FLDHARDNAME", "FLDQUICKNAME" };
        string[] alCaptions = { "Account Name", "SOA Reference", "Month", "Year" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());


        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = new DataSet();
        ds = PhoenixAccountsSOAGeneration.SOAOwnerPortalSearch(new Guid(ViewState["debitnotereferenceid"].ToString()));

        gvOwner.DataSource = ds;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvOwnersAccount", "Owners SOA With Supportings", alCaptions, alColumns, ds);
        ViewState["CURRENTYEARCODE"] = null;
        ViewState["CURRENTMONTHCODE"] = null;
    }

    private void BindCombinedPDF()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDDESCRIPTION", "FLDDEBITNOTEREFERENCE", "FLDHARDNAME", "FLDQUICKNAME" };
        string[] alCaptions = { "Account Name", "SOA Reference", "Month", "Year" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());


        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixAccountsDebitNoteReference.DebitNoteReferenceEdit(new Guid(ViewState["debitnotereferenceid"].ToString()));
        gvCombinedPDF.DataSource = ds;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvOwnersAccount", "Owners SOA With Supportings", alCaptions, alColumns, ds);
        ViewState["CURRENTYEARCODE"] = null;
        ViewState["CURRENTMONTHCODE"] = null;

    }



    protected void AttachmentList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToString().ToUpper() == cmdname)
        {
            if (!string.IsNullOrEmpty(attachmentcode))
            {

                try
                {
                    int? vesselid = null;
                    string type = Request.QueryString["type"] != null ? Request.QueryString["type"] : string.Empty;

                    if (ViewState["maxnoofattachments"] != null && ViewState["maxnoofattachments"].ToString() != "")
                    {
                        if (ViewState["ROWCOUNT"].ToString().Equals(ViewState["maxnoofattachments"]))
                        {
                            ucError.ErrorMessage = "You cannot upload more than " + ViewState["maxnoofattachments"] + " attachments.";
                            ucError.Visible = true;
                            return;
                        }
                    }

                    HttpFileCollection file = Request.Files;
                    HttpPostedFile filename = file.Get(0);
                    string fname = filename.FileName;
                    string mimetype = ".pdf";

                    DataTable dt = new DataTable();
                    dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(attachmentcode), General.GetNullableString(type));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (fname == dt.Rows[i]["FLDFILENAME"].ToString())
                        {
                            ucError.ErrorMessage = "Filename alredy exist.";
                            ucError.Visible = true;
                            return;
                        }
                    }

                    vesselid = 0;
                    PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(attachmentcode), module, null, mimetype, string.Empty, General.GetNullableString(type), vesselid);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                             "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);

                    PhoenixCommonAcount.UpdateDirectPOInvoiceStatus(new Guid(attachmentcode));
                    PhoenixCommonAcount.UpdateStatementReference(new Guid(attachmentcode));
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "parent.fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
                    BindManualAndAdditional();
                    gvAttachment.Rebind();
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
                //BindManualAndAdditional();
                //gvAttachment.Rebind();

            }
            else
            {
                string msg = "Select a record to add attachment";
                msg += string.IsNullOrEmpty(Request.QueryString["mod"]) ? "<br/>* Module is required for attachment" : string.Empty;
                ucError.ErrorMessage = msg;
                ucError.Visible = true;
            }

        }

    }
    private void BindManualAndAdditional()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["DTKey"].ToString() != string.Empty)
        {
            string type = Request.QueryString["type"] != null ? Request.QueryString["type"] : string.Empty;

            ds = PhoenixCommonFileAttachment.AttachmentSearch(new Guid(ViewState["DTKey"].ToString()), null, type
                                                                      , null
                                                                      , sortexpression, sortdirection,
                                                                      Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                      General.ShowRecords(null),
                                                                      ref iRowCount, ref iTotalPageCount);
        }

        gvAttachment.DataSource = ds;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }


    protected void gvAttachment_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            if (_gridView.EditIndex > -1)
                _gridView.UpdateRow(_gridView.EditIndex, false);

            _gridView.EditIndex = e.NewEditIndex;
            BindData();
            ((CheckBox)_gridView.Rows[e.NewEditIndex].FindControl("chkSynch")).Focus();
            gvAttachment.Rebind();
            RadLabel lblFileName = ((RadLabel)_gridView.Rows[e.NewEditIndex].FindControl("lblFileName"));
            System.Web.UI.WebControls.Image imgtype = (System.Web.UI.WebControls.Image)_gridView.Rows[e.NewEditIndex].FindControl("imgfiletype");
            if (lblFileName.Text != string.Empty)
            {
                imgtype.ImageUrl = ResolveImageType(lblFileName.Text.Substring(lblFileName.Text.LastIndexOf('.')));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }






    private string ResolveImageType(string extn)
    {
        string imagepath = Session["images"] + "/";
        if (string.IsNullOrEmpty(extn)) extn = string.Empty;
        switch (extn.ToLower())
        {
            case ".jpg":
            case ".png":
            case ".gif":
            case ".bmp":
                imagepath += "imagefile.png";
                break;
            case ".doc":
            case ".docx":
                imagepath += "word.png";
                break;
            case ".xls":
            case ".xlsx":
            case ".xlsm":
                imagepath += "xls.png";
                break;
            case ".pdf":
                imagepath += "pdf.png";
                break;
            case ".rar":
            case ".zip":
                imagepath += "rar.png";
                break;
            case ".txt":
                imagepath += "notepad.png";
                break;
            default:
                imagepath += "anyfile.png";
                break;
        }
        return imagepath;
    }
    private Guid? BindExcelReports()
    {
        FontFactory.RegisterDirectories();
        Font fontNormal = new Font(FontFactory.GetFont("Arial", 12, Font.NORMAL));

        DataSet dt = PhoenixAccountsOwnerReportDisplay.OwnerDisplayReportAditionalAttachmentList(int.Parse(ViewState["Ownerid"].ToString()));

        string[] alColumns = { "FLDVOUCHERDATE", "FLDVOUCHERROW", "FLDLONGDESCRIPTION", "FLDDEBIT", "FLDCREDIT", "FLDAMOUNT" };
        string[] alCaptions = { "DATE", "REF. NO.", "PARTICULARS", "DEBIT", "CREDIT", "BALANCE" };

        NameValueCollection nvc = new NameValueCollection();

        nvc.Add("accountId", ViewState["accountid"].ToString());
        nvc.Add("vessel", "");
        nvc.Add("month", "");
        nvc.Add("year", "");
        nvc.Add("debitnotereference", ViewState["debitnotereference"].ToString());
        nvc.Add("debitnoteid", ViewState["debitnotereferenceid"].ToString());
        nvc.Add("ownerid", ViewState["Ownerid"].ToString());


        DataSet ds = PhoenixAccountsDebitNoteReference.DebitNoteReferenceEdit(new Guid(ViewState["debitnotereferenceid"].ToString()));

        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                DateTime date = DateTime.Parse(ds.Tables[0].Rows[0]["FLDLASTDATE"].ToString());
                nvc.Add("AsOnDate", date.ToShortDateString());
                nvc.Add("AccountCode", ds.Tables[0].Rows[0]["FLDACCOUNTCODE"].ToString());
                nvc.Remove("month");
                nvc.Add("month", ds.Tables[0].Rows[0]["FLDMONTH"].ToString());
                nvc.Remove("year");
                nvc.Add("year", ds.Tables[0].Rows[0]["FLDYEAR"].ToString());
                //nvc.Add("monthname", ds.Tables[0].Rows[0]["FLDMONTHNAME"].ToString());
                //nvc.Add("yearname", ds.Tables[0].Rows[0]["FLDYEARNAME"].ToString());
            }
        }

        nvc.Add("type", "");

        Guid? returndtkey = null;

        if (dt.Tables.Count > 0)
        {
            if (dt.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                {
                    Guid dtkey = Guid.NewGuid();
                    if (dt.Tables[0].Rows[i]["FLDSUBREPORTCODE"].ToString() == "OSRM")
                    {
                        PhoenixAccounts2XL.Export2XLMMSLVoucherDetails(General.GetNullableInteger(nvc["accountId"].ToString()), General.GetNullableString(nvc["AccountCode"].ToString()),
                                                                                    General.GetNullableString(nvc["debitnotereference"].ToString()), General.GetNullableInteger(nvc["ownerid"].ToString()), dtkey);

                        long length = new System.IO.FileInfo(PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + "Accounts" + dtkey + ".xlsx").Length;
                        // long length = new System.IO.FileInfo(HttpContext.Current.Server.MapPath("..\\Attachments\\Accounts\\") + dtkey + ".xlsx").Length;

                        PhoenixCommonFileAttachment.InsertAttachment(new Guid(attachmentcode), ViewState["debitnotereference"].ToString() + ".xlsx", "Accounts/" + dtkey.ToString() + ".xlsx", length, null, 0, null, dtkey);
                        returndtkey = dtkey;

                    }

                    if (dt.Tables[0].Rows[i]["FLDSUBREPORTCODE"].ToString() == "OSRK")
                    {

                        PhoenixAccounts2XL.Export2XLKoyoVoucherDetails(General.GetNullableInteger(nvc["accountId"].ToString()), General.GetNullableString(nvc["AccountCode"].ToString()),
                                                                                  General.GetNullableString(nvc["debitnotereference"].ToString()), General.GetNullableInteger(nvc["ownerid"].ToString()), dtkey);

                        long length = new System.IO.FileInfo(PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + "Accounts" + dtkey + ".xlsx").Length;
                        //long length = new System.IO.FileInfo(HttpContext.Current.Server.MapPath("..\\Attachments\\Accounts\\") + dtkey + ".xlsx").Length;

                        PhoenixCommonFileAttachment.InsertAttachment(new Guid(attachmentcode), ViewState["debitnotereference"].ToString() + ".xlsx", "Accounts/" + dtkey.ToString() + ".xlsx", length, null, 0, null, dtkey);
                        returndtkey = dtkey;
                    }

                    if (dt.Tables[0].Rows[i]["FLDSUBREPORTCODE"].ToString() == "OSRU")
                    {

                        PhoenixAccountsSOAUACCReport2XL.Export2XLSOAUACC(General.GetNullableInteger(nvc["year"].ToString()), General.GetNullableInteger(nvc["ownerid"].ToString()), 0, General.GetNullableInteger(ViewState["vesselid"].ToString()), ViewState["yearname"].ToString(), dtkey);
                        long length = new System.IO.FileInfo(PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + "Accounts" + dtkey + ".xlsx").Length;
                        //long length = new System.IO.FileInfo(HttpContext.Current.Server.MapPath("..\\Attachments\\Accounts\\") + dtkey + ".xlsx").Length;

                        PhoenixCommonFileAttachment.InsertAttachment(new Guid(attachmentcode), ViewState["debitnotereference"].ToString() + ".xlsx", "Accounts/" + dtkey.ToString() + ".xlsx", length, null, 0, null, dtkey);
                        returndtkey = dtkey;
                    }

                    if (dt.Tables[0].Rows[i]["FLDSUBREPORTCODE"].ToString() == "OSRT")
                    {
                        returndtkey = PhoenixAccountsTRAFIGURAReport2XL.Export2XLTRAFIGURA(new Guid(ViewState["debitnotereferenceid"].ToString()), General.GetNullableInteger(nvc["ownerid"].ToString()), General.GetNullableInteger(ViewState["accountid"].ToString()), dtkey, new Guid(attachmentcode));
                        //PhoenixAccountsTRAFIGURAReport2XL.Export2XLTRAFIGURA(new Guid(ViewState["debitnotereferenceid"].ToString()), General.GetNullableInteger(nvc["ownerid"].ToString()), General.GetNullableInteger(ViewState["accountid"].ToString()), dtkey, new Guid(attachmentcode));
                    }

                    if (dt.Tables[0].Rows[i]["FLDSUBREPORTCODE"].ToString() == "OSRD")
                    {
                        PhoenixAccountsSOADiamondReport2XL.Export2XLDiamondVoucherDetails(General.GetNullableInteger(nvc["year"].ToString()), General.GetNullableInteger(nvc["ownerid"].ToString()), 0, dtkey);
                        long length = new System.IO.FileInfo(PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + "Accounts" + dtkey + ".xlsx").Length;
                        //long length = new System.IO.FileInfo(HttpContext.Current.Server.MapPath("..\\Attachments\\Accounts\\") + dtkey + ".xlsx").Length;

                        PhoenixCommonFileAttachment.InsertAttachment(new Guid(attachmentcode), ViewState["debitnotereference"].ToString() + ".xlsx", "Accounts/" + dtkey.ToString() + ".xlsx", length, null, 0, null, dtkey);
                        returndtkey = dtkey;
                    }

                    //if (dt.Tables[0].Rows[i]["FLDSUBREPORTCODE"].ToString() == "OSRC")
                    //{
                    //    //string headyear = ucFinancialYear.SelectedText;
                    //    //PhoenixAccountsSOAUACCReport2XL.Export2XLUACC(General.GetNullableInteger(ucFinancialYear.SelectedValue), General.GetNullableInteger(ucOwner.SelectedAddress), 0, General.GetNullableInteger(ddlvessel.SelectedValue), headyear);
                    //}

                    //FileInfo f = new FileInfo(filename);
                    //long s1 = f.Length;
                    //if (dt.Rows.Count == 0)
                    //{
                    //    PhoenixCommonFileAttachment.InsertAttachment(UBatchId, filepathext, "ACCOUNTS/" + filepathext, s1, null, null, "REMITTANCEBATCH", new Guid(ds.Tables[0].Rows[0]["FLDBATCHID"].ToString()));
                    //}

                    //long length = new System.IO.FileInfo(HttpContext.Current.Server.MapPath("..\\Attachments\\Accounts\\") + dtkey + ".xlsx").Length;

                    //PhoenixCommonFileAttachment.InsertAttachment(new Guid(attachmentcode), ViewState["debitnotereference"].ToString() + ".xlsx", "Accounts/" + dtkey.ToString() + ".xlsx",length, null, 0, null, dtkey);
                    //returndtkey = dtkey;

                }

            }
        }

        return returndtkey;
    }


    private void ReportGenerationPopup(string subreportcode)
    {
        String scriptpopup = "";
        if (subreportcode == "SENE")
        {
            scriptpopup = String.Format(
               "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=5&reportcode=STATEMENTOFACCOUNTSUMMARYESMBUDGET&Debitnotereference=" + ViewState["debitnotereference"].ToString() + "&debitnoteid=" + ViewState["debitnotereferenceid"].ToString() + "&showmenu=0&ownerid=" + ViewState["Ownerid"].ToString() + "&type =STATEMENTOFACCOUNTSUMMARYESMBUDGET&subreportcode=SENE');");
        }
        if (subreportcode == "SENO")
        {
            scriptpopup = String.Format(
               "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=5&reportcode=STATEMENTOFOWNERACCOUNTSUMMARY&Debitnotereference=" + General.GetNullableString(ViewState["debitnotereference"].ToString()) + "&debitnoteid=" + ViewState["debitnotereferenceid"].ToString() + "&showmenu=0&ownerid=" + ViewState["Ownerid"].ToString() + "&type =STATEMENTOFOWNERACCOUNTSUMMARY&subreportcode=SENO');");
        }
        if (subreportcode == "VTBA")
        {
            scriptpopup = String.Format(
                "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=5&reportcode=VESSELTRAILBALANCE&accountId=" + General.GetNullableInteger(ViewState["accountid"].ToString()) + "&month=" + ViewState["month"].ToString() + "&year=" + ViewState["year"].ToString() + "&debitnotereference=" + General.GetNullableString(ViewState["debitnotereference"].ToString()) + "&debitnoteid=" + General.GetNullableString(ViewState["debitnotereferenceid"].ToString()) + "&showmenu=0&ownerid=" + ViewState["Ownerid"].ToString() + "&type=VESSELTRAILBALANCE&subreportcode=VTBA');");
        }
        if (subreportcode == "VTBY")
        {
            scriptpopup = String.Format(
                "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=5&reportcode=VESSELTRAILBALANCEYTD&accountId=" + General.GetNullableInteger(ViewState["accountid"].ToString()) + "&month=" + ViewState["month"].ToString() + "&year=" + ViewState["year"].ToString() + "&debitnotereference=" + ViewState["debitnotereference"].ToString() + "&debitnoteid=" + General.GetNullableString(ViewState["debitnotereferenceid"].ToString()) + "&ownerid=" + ViewState["Ownerid"].ToString() + "&showmenu=0&subreportcode=" + subreportcode + "');");
        }
        if (subreportcode == "MVRE")
        {
            scriptpopup = String.Format(
                "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=5&reportcode=VESSELVARIANCE&vessel=null&type=Monthly&AsOnDate=" + DateTime.Parse(ViewState["AsOnDate"].ToString()).ToShortDateString() + "&debitnotereference=" + ViewState["debitnotereference"].ToString() + "&debitnoteid=" + ViewState["debitnotereferenceid"].ToString() + "&subreportcode=" + subreportcode + "&ownerid=" + ViewState["Ownerid"].ToString() + "&showmenu=0');");
        }
        if (subreportcode == "YVRE")
        {
            scriptpopup = String.Format(
                       "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=5&reportcode=VESSELVARIANCE&vessel=null&type=Yearly&AsOnDate=" + DateTime.Parse(ViewState["AsOnDate"].ToString()).ToShortDateString() + "&debitnotereference=" + ViewState["debitnotereference"].ToString() + "&debitnoteid=" + ViewState["debitnotereferenceid"].ToString() + "&subreportcode=" + subreportcode + "&ownerid=" + ViewState["Ownerid"].ToString() + "&showmenu=0');");
        }
        if (subreportcode == "AVRE")
        {
            scriptpopup = String.Format(
                       "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=5&reportcode=VESSELVARIANCE&vessel=null&type=Accumulated&AsOnDate=" + DateTime.Parse(ViewState["AsOnDate"].ToString()).ToShortDateString() + "&debitnotereference=" + ViewState["debitnotereference"].ToString() + "&debitnoteid=" + ViewState["debitnotereferenceid"].ToString() + "&subreportcode=" + subreportcode + "&ownerid=" + ViewState["Ownerid"].ToString() + "&showmenu=0');");
        }
        if (subreportcode == "FDP")
        {
            scriptpopup = String.Format(
                       "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=5&reportcode=VESSELSUMMARYBALANCE&vessel=" + null + "&month=" + ViewState["month"].ToString() + "&year=" + ViewState["year"].ToString() + "&debitnotereference=" + General.GetNullableString(ViewState["debitnotereference"].ToString()) + "&debitnoteid=" + General.GetNullableString(ViewState["debitnotereferenceid"].ToString()) + "&ownerid=" + ViewState["Ownerid"].ToString() + "&subreportcode=FDP&showmenu=0&type=VESSELSUMMARYBALANCE');");
        }
        if (subreportcode == "VTBO")
        {
            scriptpopup = String.Format(
                "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=5&reportcode=VESSELTRAILBALANCEYTDOWNER&accountId=" + General.GetNullableInteger(ViewState["accountid"].ToString()) + "&debitnoteid=" + General.GetNullableString(ViewState["debitnotereferenceid"].ToString()) + "&ownerid=" + ViewState["Ownerid"].ToString() + "&showmenu=0&subreportcode=" + subreportcode + "');");
        }
        if (subreportcode == "SENWB")
        {
            scriptpopup = String.Format(
               "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=5&reportcode=STATEMENTOFOWNERACCOUNTWITHOUTBUDGET&Debitnotereference=" + General.GetNullableString(ViewState["debitnotereference"].ToString()) + "&debitnoteid=" + ViewState["debitnotereferenceid"].ToString() + "&showmenu=0&ownerid=" + ViewState["Ownerid"].ToString() + "&type =STATEMENTOFOWNERACCOUNTWITHOUTBUDGET&subreportcode=SENWB');");
        }
        if (scriptpopup != "")
        {
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
        }

    }

    private bool SOALevelValidation(Guid DebitNoteID)
    {
        bool result = false;

        DataTable dt = PhoenixAccountsSOAGeneration.SOAGenerationStatusUpdateValidation(DebitNoteID);
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0][0].ToString() == "1")
            {
                result = true;
            }
        }
        return result;
    }

    protected void SOASchedule_Click(object sender, EventArgs e)
    {
        try
        {
            //UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;
            //if (ucCM.confirmboxvalue == 1)
            //{
            PhoenixAccountsDebitNoteReference.SOAGenerationCombinedPDFScheduleTaskEntry(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["debitnotereferenceid"].ToString()));
            ucStatus.Text = "Schedule entry updated. Combined pdf will generated in schedule";
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void CheckMapping_Click(object sender, EventArgs e)
    {
        try
        {
            //UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;
            //if (ucCM.confirmboxvalue == 1)
            //{
            PhoenixAccountsDebitNoteReference.DebitNoteReference1stCheckUpdate(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , General.GetNullableGuid(ViewState["debitnotereferenceid"].ToString()));
            ucStatus.Text = "Debit Note Reference 1st Level Checked.";
            BindSOADetails();
            StatusbasedVisibilty();
            // }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucConfirm2ndlevel_Click(object sender, EventArgs e)
    {
        try
        {
            //UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;
            //if (ucCM.confirmboxvalue == 1)
            //{
            PhoenixAccountsDebitNoteReference.DebitNoteReference2ndCheckUpdate(
                 PhoenixSecurityContext.CurrentSecurityContext.UserCode
                 , General.GetNullableGuid(ViewState["debitnotereferenceid"].ToString()));
            ucStatus.Text = "Debit Note Reference 2nd Level Checked.";
            BindSOADetails();
            StatusbasedVisibilty();
            // }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucConfirmpublish_Click(object sender, EventArgs e)
    {
        try
        {
            //UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;
            //if (ucCM.confirmboxvalue == 1)
            //{
            PhoenixAccountsDebitNoteReference.DebitNoteReferencePublish(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , General.GetNullableGuid(ViewState["debitnotereferenceid"].ToString()));
            ucStatus.Text = "Debit Note Reference published.";
            BindSOADetails();
            StatusbasedVisibilty();
            // }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucConfirmAddlreattch_Click(object sender, EventArgs e)
    {
        try
        {
            //UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;
            //if (ucCM.confirmboxvalue == 1)
            //{
            Guid? addlDTKey = BindExcelReports();
            if (addlDTKey != null)
            {
                PhoenixCommonAcount.UpdateStatementReference(new Guid(ViewState["DTKey"].ToString()));
                PhoenixAccountsSOAGeneration.SOAGenerationAdditionalAttachmentUpdation(new Guid(ViewState["debitnotereferenceid"].ToString()), addlDTKey);
                PhoenixCommonFileAttachment.AttachmentDelete(new Guid(ViewState["AddlDTKey"].ToString()));
                BindManualAndAdditional();
            }
            // }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindSOADetails()
    {
        if (ViewState["debitnotereferenceid"] != null && ViewState["debitnotereferenceid"].ToString() != "")
        {
            DataTable ds = PhoenixAccountsSOAGeneration.SOAGenerationDetails(new Guid(ViewState["debitnotereferenceid"].ToString()));

            if (ds.Rows.Count > 0)
            {
                ViewState["debitnotereference"] = ds.Rows[0]["FLDDEBITNOTEREFERENCE"].ToString();
                ViewState["accountid"] = ds.Rows[0]["FLDACCOUNTID"].ToString();
                ViewState["Ownerid"] = ds.Rows[0]["FLDOWNERID"].ToString();
                ViewState["AsOnDate"] = ds.Rows[0]["FLDLASTDATE"].ToString();
                ViewState["year"] = ds.Rows[0]["FLDYEAR"].ToString();
                ViewState["month"] = ds.Rows[0]["FLDMONTH"].ToString();
                ViewState["DTKey"] = ds.Rows[0]["FLDDTKEY"].ToString();
                ViewState["URL"] = ds.Rows[0]["FLDURL"].ToString();
                ViewState["AddlDTKey"] = ds.Rows[0]["FLDOWNERSPECREPDTKEY"].ToString();
                ViewState["SOAStatus"] = ds.Rows[0]["FLDSTATUS"].ToString();
            }
            ViewState["FLDVOUCHERDETAILID"] = "";
        }
    }

    private void StatusbasedVisibilty()
    {
        if (ViewState["SOAStatus"] != null)
        {
            if (ViewState["debitnotereference"] != null)
            {
                lblSOA.Text = ViewState["debitnotereference"].ToString() + " - " + ViewState["SOAStatus"].ToString();
            }

            PhoenixToolbar toolbarlineitem = new PhoenixToolbar();

            toolbarlineitem.AddButton("Finalize Attachment", "FINALIZEATTACHMNENTS", ToolBarDirection.Right);

            if (ViewState["SOAStatus"].ToString() == "Published")
                toolbarlineitem.AddButton("Un-Publish", "UNPUBLISH", ToolBarDirection.Right);

            if (ViewState["SOAStatus"].ToString() == "2nd Level Checked")
                toolbarlineitem.AddButton("Publish", "PUBLISH", ToolBarDirection.Right);

            if (ViewState["SOAStatus"].ToString() == "1st Level Checked")
                toolbarlineitem.AddButton("2nd Level Check", "2NDCHECK", ToolBarDirection.Right);

            if (ViewState["SOAStatus"].ToString() == "Pending")
                toolbarlineitem.AddButton("1st Level Check", "1STCHECK", ToolBarDirection.Right);

            toolbarlineitem.AddButton("Generate Additional Attachments", "ADDLATTACHMNENTS", ToolBarDirection.Right);

            if (ViewState["SOAStatus"].ToString() != "Published")
            {
                toolbarlineitem.AddButton("Generate Combined PDF", "COMBINEDPDF", ToolBarDirection.Right);
                toolbarlineitem.AddButton("Schedule Combined PDF", "SCHEDULEENTRY", ToolBarDirection.Right);
            }





            MenuSOALineItems.AccessRights = this.ViewState;
            MenuSOALineItems.MenuList = toolbarlineitem.Show();

            if (ViewState["SOAStatus"].ToString() != "Published")
            {
                PhoenixToolbar toolbarmainatt = new PhoenixToolbar();
                toolbarmainatt.AddButton("Upload", cmdname);
                AttachmentList.AccessRights = this.ViewState;
                AttachmentList.MenuList = toolbarmainatt.Show();
            }
        }

    }



    private void BindManualReportData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSUBREPORTTYPE", "FLDSUBREPORTTYPE" };
        string[] alCaptions = { "Sub Report", "Sub Report Code" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = new DataSet();
        ds = PhoenixAccountsSOAGeneration.SOAGenerationManualTypeReportList(new Guid(ViewState["debitnotereferenceid"].ToString()));

        gvManualReports.DataSource = ds;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvManualReports", "Manual Report List", alCaptions, alColumns, ds);

    }


    protected void gvOwnersAccount_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvOwnersAccount_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;


        if (e.CommandName.ToUpper().Equals("REPORT"))
        {
            RadLabel lbl = ((RadLabel)e.Item.FindControl("lblReportCode"));
            ReportGenerationPopup(lbl.Text);
        }
    }

    protected void gvCombinedPDF_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindCombinedPDF();
    }

    protected void gvCombinedPDF_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;


        if (e.CommandName.ToUpper().Equals("PDFVIEW"))
        {
            String scriptpopup = "";
            scriptpopup = String.Format(
                "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsSOAGenerationPdfReport.aspx?pdfGen=0&debitnotereference=" + ViewState["debitnotereference"] + "&debitnotereferenceid=" + ViewState["debitnotereferenceid"] + "&accountid="
                                           + ViewState["accountid"] + "&Ownerid=" + ViewState["Ownerid"] + "&dtkey=" + ViewState["DTKey"].ToString() + "&description=" + ViewState["description"] + "&year=" + ViewState["year"] + "&month=" + ViewState["month"] + "&Type=0&URL=" + ViewState["URL"] + "');");
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

        }
        if (e.CommandName.ToUpper().Equals("UPDATE"))
        {
            PhoenixAccountsSOAGeneration.SOAGenerationCombinedPDFUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["debitnotereferenceid"].ToString()), ((RadTextBox)e.Item.FindControl("txtURL")).Text, 1);

            BindCombinedPDF();
            gvCombinedPDF.Rebind();

        }
    }

    protected void gvOwner_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindOwnerData();
    }

    protected void gvOwner_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;



        String scriptpopup = "";
        if (e.CommandName.ToUpper().Equals("SELECT"))
        {
            RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
            ViewState["accountid"] = (RadLabel)e.Item.FindControl("lblAccountId");

            RadLabel lblAccountCode = (RadLabel)e.Item.FindControl("lblAccountCode");

            RadLabel lblDebitNoteReference = (RadLabel)e.Item.FindControl("lblSoaReference");
            ViewState["debitnotereference"] = (RadLabel)e.Item.FindControl("lblSoaReference");

            RadLabel lblOwnerid = (RadLabel)e.Item.FindControl("lblOwnerId");
            ViewState["Ownerid"] = (RadLabel)e.Item.FindControl("lblOwnerId");

            RadLabel lblDebitNoteReferenceID = (RadLabel)e.Item.FindControl("lblSoaReferenceid");
            ViewState["debitnotereferenceID"] = (RadLabel)e.Item.FindControl("lblSoaReferenceid");

            Response.Redirect("../Accounts/AccountsOwnerStatementOfAccountBudget.aspx?accountid="
                 + lblAccountId.Text + "&debitnoteref="
                 + lblDebitNoteReference.Text + "&accountcode="
                 + lblAccountCode.Text + "&debitnoterefID="
                 + lblDebitNoteReferenceID.Text + "&from=SOAGeneration", true);

        }

        if (e.CommandName.ToUpper().Equals("EXCEL"))
        {
            RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
            RadLabel lblDebitNoteReference = (RadLabel)e.Item.FindControl("lblSoaReference");
            RadLabel lblOwnerid = (RadLabel)e.Item.FindControl("lblOwnerId");

            DataSet ds = new DataSet();

            ds = PhoenixAccountsOwnerStatementOfAccount.StatementOfAccountsVoucher(lblDebitNoteReference.Text, General.GetNullableInteger(lblOwnerid.Text), General.GetNullableInteger(lblAccountId.Text));

            if (ds.Tables[0].Rows.Count > 0)
            {

                string[] alColumns = { "FLDOWNERBUDGETCODE", "FLDDESCRIPTION", "FLDVOUCHERDATE", "FLDVOUCHERROW", "FLDLONGDESCRIPTION", "FLDAMOUNT" };
                string[] alCaptions = { "Owner Budget Code", "Owner Budget Code Description", "Voucher Date", "Voucher Row", "Particulars", "Amount(USD)" };
                string prevBcode = null;
                string prevBdescription = null;
                string curBcode = null;
                string curBdescription = null;

                string filename = ds.Tables[0].Rows[0]["FLDOWNERBUDGETCODE"].ToString() + "_" + ds.Tables[0].Rows[0]["FLDDEBITNOTEREFERENCE"].ToString() + ".xls";
                filename = filename.Replace(" ", "");

                Response.AddHeader("Content-Disposition", "attachment; filename=" + filename.ToString());       //" + ds.Tables[0].Rows[0]["FLDOWNERBUDGETCODE"].ToString() + "_" + ds.Tables[0].Rows[0]["FLDDEBITNOTEREFERENCE"].ToString() + ".xls");
                Response.ContentType = "application/vnd.msexcel";
                Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
                Response.Write("<tr>");
                Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
                Response.Write("<h3><center>" + ds.Tables[0].Rows[0]["FLDACCOUNTCODEDESC"].ToString() + "<br/>" + ds.Tables[0].Rows[0]["FLDDEBITNOTEREFERENCE"].ToString() + "</center></h3></td>");
                //Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
                Response.Write("</tr>");
                Response.Write("</TABLE>");
                Response.Write("<br />");
                Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
                Response.Write("<tr>");
                for (int i = 0; i < alCaptions.Length; i++)
                {

                    if (i == 4)
                        Response.Write("<td colspan='2'>");
                    else
                        Response.Write("<td width='15%'>");
                    Response.Write("<b>" + alCaptions[i].ToString().Trim() + "</b>");
                    Response.Write("</td>");
                }
                Response.Write("</tr>");
                DataTable dt = ds.Tables[0];
                for (int dr = 0; dr < dt.Rows.Count; dr++)
                {
                    DataRow previousdatarow;
                    DataRow datarow;
                    Response.Write("<tr>");
                    for (int i = 0; i < alColumns.Length; i++)
                    {
                        if (i == 4)
                            Response.Write("<td colspan='2'>");
                        else
                            Response.Write("<td width='15%'>");
                        decimal s;

                        if (dr > 0)
                        {

                            previousdatarow = dt.Rows[dr - 1];
                            datarow = dt.Rows[dr];
                            prevBcode = datarow[alColumns[0]].ToString();
                            prevBdescription = datarow[alColumns[1]].ToString();
                            curBcode = previousdatarow[alColumns[0]].ToString();
                            curBdescription = previousdatarow[alColumns[1]].ToString();

                            Response.Write("<font color='black'>");

                        }
                        else
                        {
                            datarow = dt.Rows[dr];
                            Response.Write("<font color='black'>");
                        }


                        Response.Write(decimal.TryParse(datarow[alColumns[i]].ToString(), out s) ? String.Format("{0:#,###,###.##}", datarow[alColumns[i]]) : datarow[alColumns[i]]);
                        Response.Write("</font>");
                        Response.Write("</td>");

                    }
                    Response.Write("</tr>");
                }
                Response.Write("</TABLE>");
                Response.Write("</body>");
                Response.Write("</html>");
                Response.End();

            }

        }
        if (e.CommandName.ToUpper().Equals("MONTHLYVARIANCE"))
        {
            RadLabel lblDebitNoteReference = (RadLabel)e.Item.FindControl("lblSoaReference");
            RadLabel lblDebitNoteReferenceID = (RadLabel)e.Item.FindControl("lblSoaReferenceID");
            RadLabel lblAsonDate = (RadLabel)e.Item.FindControl("lblASonDate");
            RadLabel lblOwnerid = (RadLabel)e.Item.FindControl("lblOwnerId");

            DateTime date = DateTime.Parse(lblAsonDate.Text);

            scriptpopup = String.Format(
                "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=5&reportcode=VESSELVARIANCE&vessel=null&type=Monthly&AsOnDate=" + date.ToShortDateString() + "&debitnotereference=" + lblDebitNoteReference.Text + "&debitnoteid=" + lblDebitNoteReferenceID.Text + "&showmenu=0&ownerid=" + lblOwnerid.Text + "&subreportcode=MVRE');");
        }

        if (e.CommandName.ToUpper().Equals("YEARLYVARIANCE"))
        {
            RadLabel lblDebitNoteReference = (RadLabel)e.Item.FindControl("lblSoaReference");
            RadLabel lblDebitNoteReferenceID = (RadLabel)e.Item.FindControl("lblSoaReferenceID");
            RadLabel lblAsonDate = (RadLabel)e.Item.FindControl("lblASonDate");
            RadLabel lblOwnerid = (RadLabel)e.Item.FindControl("lblOwnerId");

            DateTime date = DateTime.Parse(lblAsonDate.Text);

            scriptpopup = String.Format(
                       "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=5&reportcode=VESSELVARIANCE&vessel=null&type=Yearly&AsOnDate=" + date.ToShortDateString() + "&debitnotereference=" + lblDebitNoteReference.Text + "&debitnoteid=" + lblDebitNoteReferenceID.Text + "&showmenu=0&ownerid=" + lblOwnerid.Text + "&subreportcode=YVRE');");
        }

        if (e.CommandName.ToUpper().Equals("ACCUMALTEDVARIANCE"))
        {
            RadLabel lblDebitNoteReference = (RadLabel)e.Item.FindControl("lblSoaReference");
            RadLabel lblDebitNoteReferenceID = (RadLabel)e.Item.FindControl("lblSoaReferenceID");
            RadLabel lblAsonDate = (RadLabel)e.Item.FindControl("lblASonDate");
            RadLabel lblOwnerid = (RadLabel)e.Item.FindControl("lblOwnerId");

            DateTime date = DateTime.Parse(lblAsonDate.Text);


            scriptpopup = String.Format(
                       "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=5&reportcode=VESSELVARIANCE&vessel=null&type=Accumulated&AsOnDate=" + date.ToShortDateString() + "&debitnotereference=" + lblDebitNoteReference.Text + "&debitnoteid=" + lblDebitNoteReferenceID.Text + "&showmenu=0&ownerid=" + lblOwnerid.Text + "&subreportcode=AVRE');");
        }

        if (scriptpopup != "")
        {
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
        }

    }

    protected void gvOwner_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
            RadLabel lblDebitNoteReference = (RadLabel)e.Item.FindControl("lblSoaReference");
            RadLabel lblURL = (RadLabel)e.Item.FindControl("lblURL");
            RadLabel lblOwnerid = (RadLabel)e.Item.FindControl("lblOwnerId");

            RadLabel lblDebitNoteReferenceid = (RadLabel)e.Item.FindControl("lblSoaReferenceid");

            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkAccountCOde");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            ImageButton att = (ImageButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (drv["FLDISATTACHMENT"].ToString() == "0")
                    att.ImageUrl = Session["images"] + "/no-attachment.png";
                att.Attributes.Add("onclick", "javascript:parent.openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.ACCOUNTS + "&U=NO'); return false;");
            }
            DataTable dt;
            if (!string.IsNullOrEmpty(lblOwnerid.Text))
            {
                LinkButton cmdTBPdf = (LinkButton)e.Item.FindControl("cmdTBPdf");
                if (cmdTBPdf != null)
                {

                    dt = PhoenixAccountsOwnerReportDisplay.OwnerDisplayReporPortalAccess(int.Parse(lblOwnerid.Text), "VTBA");
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["FLDACCESSYN"].ToString() == "1")
                        {
                            cmdTBPdf.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                            cmdTBPdf.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=5&reportcode=VESSELTRAILBALANCE&accountId=" + General.GetNullableInteger(lblAccountId.Text) + "&month=" + null + "&year=" + null + "&debitnotereference=" + General.GetNullableString(lblDebitNoteReference.Text) + "&debitnoteid=" + General.GetNullableString(lblDebitNoteReferenceid.Text) + "&showmenu=0&ownerid=" + lblOwnerid.Text + "&type=VESSELTRAILBALANCE&subreportcode=VTBA');");
                            //BindData();
                            //BindOwnerData();
                            //BindCombinedPDF();
                        }
                    }
                }

                LinkButton cmdTBYTD = (LinkButton)e.Item.FindControl("cmdTBYTD");
                if (cmdTBYTD != null)
                {
                    dt = PhoenixAccountsOwnerReportDisplay.OwnerDisplayReporPortalAccess(int.Parse(lblOwnerid.Text), "VTBY");
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["FLDACCESSYN"].ToString() == "1")
                        {
                            cmdTBYTD.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                            cmdTBYTD.Visible = true;
                            cmdTBYTD.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=5&reportcode=VESSELTRAILBALANCEYTD&accountId=" + General.GetNullableInteger(lblAccountId.Text) + "&month=" + null + "&year=" + null + "&debitnotereference=" + General.GetNullableString(lblDebitNoteReference.Text) + "&debitnoteid=" + General.GetNullableString(lblDebitNoteReferenceid.Text) + "&showmenu=0&ownerid=" + lblOwnerid.Text + "&type=VESSELTRAILBALANCE&subreportcode=VTBY');");
                        }
                    }
                }

                LinkButton cmdSummaryPdf = (LinkButton)e.Item.FindControl("cmdSummaryPdf");
                if (cmdSummaryPdf != null)
                {
                    dt = PhoenixAccountsOwnerReportDisplay.OwnerDisplayReporPortalAccess(int.Parse(lblOwnerid.Text), "SENO");
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["FLDACCESSYN"].ToString() == "1")
                        {
                            cmdSummaryPdf.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                            cmdSummaryPdf.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=5&reportcode=STATEMENTOFOWNERACCOUNTSUMMARY&Debitnotereference=" + General.GetNullableString(lblDebitNoteReference.Text) + "&debitnoteid=" + General.GetNullableString(lblDebitNoteReferenceid.Text) + "&showmenu=0&ownerid=" + lblOwnerid.Text + "&subreportcode=SENO');");
                            //BindData();
                            //BindOwnerData();
                            //BindCombinedPDF();
                        }
                    }
                }

                ImageButton img = (ImageButton)e.Item.FindControl("cmdPdf");
                if (img != null)
                {
                    dt = PhoenixAccountsOwnerReportDisplay.OwnerDisplayReporPortalAccess(int.Parse(lblOwnerid.Text), "FDP");
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["FLDACCESSYN"].ToString() == "1")
                        {
                            img.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                            img.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=5&reportcode=VESSELSUMMARYBALANCE&vessel=" + null + "&month=" + null + "&year=" + null + "&debitnotereference=" + General.GetNullableString(lblDebitNoteReference.Text) + "&debitnoteid=" + General.GetNullableString(lblDebitNoteReferenceid.Text) + "&showmenu=0&ownerid=" + lblOwnerid.Text + "&type=VESSELSUMMARYBALANCE&subreportcode=FDP');");
                            //BindData();
                            //BindOwnerData();
                            //BindCombinedPDF();
                        }
                    }
                }

                LinkButton cmdMVR = (LinkButton)e.Item.FindControl("cmdMonthlyVariance");
                if (cmdMVR != null)
                {
                    dt = PhoenixAccountsOwnerReportDisplay.OwnerDisplayReporPortalAccess(int.Parse(lblOwnerid.Text), "MVRE");
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["FLDACCESSYN"].ToString() == "1")
                        {
                            cmdMVR.Visible = true;
                        }
                    }
                }

                LinkButton cmdYVR = (LinkButton)e.Item.FindControl("cmdYearlyVariance");
                if (cmdYVR != null)
                {
                    dt = PhoenixAccountsOwnerReportDisplay.OwnerDisplayReporPortalAccess(int.Parse(lblOwnerid.Text), "YVRE");
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["FLDACCESSYN"].ToString() == "1")
                        {
                            cmdYVR.Visible = true;
                        }
                    }
                }

                LinkButton cmdAVR = (LinkButton)e.Item.FindControl("cmdAccumaltedVariance");
                if (cmdAVR != null)
                {
                    dt = PhoenixAccountsOwnerReportDisplay.OwnerDisplayReporPortalAccess(int.Parse(lblOwnerid.Text), "AVRE");
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["FLDACCESSYN"].ToString() == "1")
                        {
                            cmdAVR.Visible = true;
                        }
                    }
                }

                LinkButton cmdTBYTDOwner = (LinkButton)e.Item.FindControl("cmdTBYTDOwner");
                if (cmdTBYTDOwner != null)
                {
                    dt = PhoenixAccountsOwnerReportDisplay.OwnerDisplayReporPortalAccess(int.Parse(lblOwnerid.Text), "VTBO");
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["FLDACCESSYN"].ToString() == "1")
                        {
                            cmdTBYTDOwner.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                            cmdTBYTDOwner.Visible = true;
                            cmdTBYTDOwner.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=5&reportcode=VESSELTRAILBALANCEYTDOWNER&accountId=" + int.Parse(lblAccountId.Text) + "&debitnoteid=" + new Guid(lblDebitNoteReferenceid.Text) + "&showmenu=0&ownerid=" + lblOwnerid.Text + "&type=VESSELTRAILBALANCE&subreportcode=VTBO');");
                        }
                    }
                }
            }

        }
    }

    protected void gvManualReports_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindManualReportData();
    }

    protected void gvManualReports_ItemCommand(object sender, GridCommandEventArgs e)
    {

        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            RadLabel lblsubreportcode = ((RadLabel)e.Item.FindControl("lblReportCode"));
            RadLabel lblManDTkey = ((RadLabel)e.Item.FindControl("lblDTKey"));

            if (e.CommandName.ToUpper().Equals("REPORT"))
            {
                //String scriptpopup = String.Format("javascript:parent.Openpopup('codehelp1', '', '../Accounts/AccountsStatementRefFileAttachment.aspx?dtkey=" + lblManDTkey.Text.ToString() + "&mod=" + PhoenixModule.ACCOUNTS + "&Status=" + ViewState["SOAStatus"] + "&debitnotereferenceid=" + ViewState["debitnotereferenceid"] + "');");
                //ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);          
                String scriptpopup = String.Format("javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsSOAGenerationManualReportsList.aspx?dtkey=" + lblManDTkey.Text.ToString() + "&mod=" + PhoenixModule.ACCOUNTS + "&Status=" + ViewState["SOAStatus"] + "&debitnotereferenceid=" + ViewState["debitnotereferenceid"] + "&subreportcode=" + lblsubreportcode.Text + "&mimetype=.pdf" + "');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

            }
            if (e.CommandName.ToUpper().Equals("VERIFY"))
            {
                DataSet ds = PhoenixReportsAccount.SOAGenerationOwnerReportVerification(int.Parse(ViewState["Ownerid"].ToString()), new Guid(ViewState["debitnotereferenceid"].ToString()), lblsubreportcode.Text);
                if (ds.Tables.Count > 0)
                {
                    ucStatus.Text = ds.Tables[0].Rows[0]["FLDMSG"].ToString();
                    ucStatus.Visible = true;
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAttachment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindManualAndAdditional();
    }

    protected void gvAttachment_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvAttachment_ItemDataBound(object sender, GridItemEventArgs e)
    {

        DataRowView drv = (DataRowView)e.Item.DataItem;


        if (e.Item is GridDataItem)
        {
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event)");

                    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                    ImageButton ed = (ImageButton)e.Item.FindControl("cmdEdit");
                    ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
                    if (Request.QueryString["u"] != null)
                    {
                        db.Visible = false;
                        ed.Visible = false;
                        e.Item.Attributes["ondblclick"] = string.Empty;
                    }
                    if (Request.QueryString["SOAStatus"] == "Published")
                    {
                        db.Visible = false;
                        ed.Visible = false;
                        e.Item.Attributes["ondblclick"] = string.Empty;
                    }

                    RadLabel lblFileName = ((RadLabel)e.Item.FindControl("lblFileName"));
                    System.Web.UI.WebControls.Image imgtype = (System.Web.UI.WebControls.Image)e.Item.FindControl("imgfiletype");
                    if (lblFileName.Text != string.Empty)
                    {
                        imgtype.ImageUrl = ResolveImageType(lblFileName.Text.Substring(lblFileName.Text.LastIndexOf('.')));

                        RadLabel lblFilePath = (RadLabel)e.Item.FindControl("lblFilePath");
                        HyperLink lnk = (HyperLink)e.Item.FindControl("lnkfilename");
                        lnk.NavigateUrl = "../Common/download.aspx?dtkey=" + drv["FLDDTKEY"].ToString();  //Session["sitepath"] + "/attachments/" + lblFilePath.Text;
                                                                                                          //lnk.NavigateUrl = Session["sitepath"] + "/attachments/" + lblFilePath.Text;
                    }
                }
            }
        }

    }

    protected void gvAttachment_UpdateCommand(object sender, GridCommandEventArgs e)
    {

        try
        {
            string dtkey = ((RadLabel)e.Item.FindControl("lblDTKeyEdit")).Text;
            bool chk = ((CheckBox)e.Item.FindControl("chkSynch")).Checked;
            PhoenixCommonFileAttachment.FileSyncUpdate(new Guid(dtkey), (chk ? byte.Parse("1") : byte.Parse("0")));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

        BindManualAndAdditional();
        gvAttachment.Rebind();
    }

    protected void gvAttachment_DeleteCommand(object sender, GridCommandEventArgs e)
    {

        try
        {
            //BindSOADetails();
            string dtkey = ((RadLabel)e.Item.FindControl("lblDTKey")).Text;
            RadLabel lblFilePath = (RadLabel)e.Item.FindControl("lblFilePath");

            if (ViewState["debitnotereferenceid"] != null && ViewState["debitnotereferenceid"].ToString() != "")
            {
                PhoenixAccountsSOAGeneration.AttachmentVerificationDetailsDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["debitnotereferenceid"].ToString()), new Guid(dtkey));
                PhoenixCommonFileAttachment.AttachmentDelete(new Guid(dtkey));

                if (ViewState["AddlDTKey"] != null)
                {
                    if (dtkey == ViewState["AddlDTKey"].ToString())
                    {
                        PhoenixAccountsSOAGeneration.SOAGenerationAdditionalAttachmentFlagReset(new Guid(ViewState["debitnotereferenceid"].ToString()));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        BindManualAndAdditional();
        gvAttachment.Rebind();
    }

    protected void confirm_Click(object sender, EventArgs e)
    {

    }
    private void FinalizeAttachments()
    {
        DataSet ds = PhoenixAccountsDebitNoteReference.DebitNoteReferenceEdit(new Guid(ViewState["debitnotereferenceid"].ToString()));
        string referencetype = "";

        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                referencetype = ds.Tables[0].Rows[0]["FLDTYPE"].ToString();
            }
        }

        DataSet dt = PhoenixAccountsOwnerReportDisplay.OwnerDisplayReportTypeList(int.Parse(ViewState["Ownerid"].ToString()), referencetype);

        if (dt.Tables.Count > 0)
        {
            if (dt.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                {
                    if (dt.Tables[0].Rows[i]["FLDSUBREPORTCODE"].ToString() == "ASLO" || dt.Tables[0].Rows[i]["FLDSUBREPORTCODE"].ToString() == "ASLE")
                        ViewState["subreportcode"] = dt.Tables[0].Rows[i]["FLDSUBREPORTCODE"].ToString();
                }
            }
        }


        if (ViewState["subreportcode"].ToString() == "ASLO")
        {
            DataTable DtBL = PhoenixAccountsSoaChecking.SoaCheckingBudgetList(int.Parse(ViewState["accountid"].ToString()), ViewState["debitnotereference"].ToString());
            if (DtBL.Rows.Count > 0)
            {
                foreach (DataRow DrBL in DtBL.Rows) // Loop over the rows.
                {
                    DataTable DtVL = PhoenixAccountsSoaChecking.SoaCheckingVouchersList(ViewState["debitnotereference"].ToString(), int.Parse(ViewState["Ownerid"].ToString()), General.GetNullableString(DrBL["FLDBUDGETID"].ToString()), int.Parse(ViewState["accountid"].ToString()));

                    if (DtVL.Rows.Count > 0)
                    {
                        string strpath, strFilePath, strdirectoryname;
                        int iRowCount = 0;
                        int iTotalPageCount = 0;
                        foreach (DataRow DrVL in DtVL.Rows) // Loop over the rows.
                        {
                            //DataSet dsVLFA = PhoenixCommonFileAttachment.AttachmentSOACheckingVoucherSearch(new Guid(DrVL["FLDVOUCHERDTKEY"].ToString()), null, null, null, null, 1, 100, ref iRowCount, ref iTotalPageCount);

                            //if (dsVLFA.Tables[0].Rows.Count > 0)
                            //{
                            //    DataTable DtVLFA = dsVLFA.Tables[0];
                            //    foreach (DataRow DrVLFA in DtVLFA.Rows) // Loop over the rows.
                            //    {

                            //        strpath = "";
                            //        strFilePath = "";
                            //        strdirectoryname = "";

                            //        strpath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + DrVLFA["FLDFILEPATH"].ToString();
                            //        strFilePath = strpath.Substring(strpath.Length - 40, 40);

                            //        strdirectoryname = strpath.Substring(0, strpath.Length - 40);
                            //        strdirectoryname = strdirectoryname + ViewState["debitnotereference"].ToString();

                            //        strFilePath = strdirectoryname + "/" + strFilePath;

                            //        if (!Directory.Exists(strdirectoryname))
                            //            Directory.CreateDirectory(strdirectoryname);


                            //        using (PdfSharp.Pdf.PdfDocument pdfDoc = PdfSharp.Pdf.IO.PdfReader.Open(PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + DrVLFA["FLDFILEPATH"].ToString(), PdfSharp.Pdf.IO.PdfDocumentOpenMode.Modify))
                            //        {

                            //            for (int i = 0; i < pdfDoc.PageCount; i++)
                            //            {
                            //                //// Variation 1: Draw a watermark as a text string.

                            //                //// Get an XGraphics object for drawing beneath the existing content.
                            //                var gfx = XGraphics.FromPdfPage(pdfDoc.Pages[i], XGraphicsPdfPageOptions.Append);
                            //                gfx.SmoothingMode = XSmoothingMode.HighQuality;



                            //                XFont font = new XFont("Times New Roman", 12, XFontStyle.BoldItalic);

                            //                // Create a string format.
                            //                var format = new XStringFormat();
                            //                format.Alignment = XStringAlignment.Near;
                            //                format.LineAlignment = XLineAlignment.Near;

                            //                // Create a dimmed red brush.
                            //                XBrush brush = new XSolidBrush(XColor.FromArgb(128, 255, 0, 0));


                            //                // Draw the string.                                           

                            //                if (DrVLFA["FLDATTACHMENTTYPE"].ToString() == "MST Acknowledgement" || DrVLFA["FLDATTACHMENTTYPE"].ToString() == "Owner Approval")
                            //                {
                            //                    if (pdfDoc.Pages[i].Orientation.ToString() == "Landscape")
                            //                    {
                            //                        gfx.DrawString((DrVL["FLDVOUCHERROW"].ToString()), font, brush, new XPoint(100, (pdfDoc.Pages[i].Height) - 40), format);
                            //                    }
                            //                    else
                            //                    {
                            //                        gfx.DrawString((DrVL["FLDVOUCHERROW"].ToString()), font, brush, new XPoint(100, (pdfDoc.Pages[i].Height) - 50), format);
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    if (pdfDoc.Pages[i].Orientation.ToString() == "Landscape")
                            //                    {

                            //                        gfx.DrawString((DrVL["FLDDESCRIPTIONFORPRINT"].ToString()), font, brush, new XPoint(100, (pdfDoc.Pages[i].Height) - 40), format);
                            //                        gfx.DrawString((DrVL["FLDREMARKS"].ToString()), font, brush, new XPoint(100, (pdfDoc.Pages[i].Height) - 30), format);
                            //                    }
                            //                    else
                            //                    {
                            //                        gfx.DrawString((DrVL["FLDDESCRIPTIONFORPRINT"].ToString()), font, brush, new XPoint(100, (pdfDoc.Pages[i].Height) - 50), format);
                            //                        gfx.DrawString((DrVL["FLDREMARKS"].ToString()), font, brush, new XPoint(100, (pdfDoc.Pages[i].Height) - 40), format);
                            //                    }
                            //                }

                            //                pdfDoc.Save(strFilePath);
                            //            }
                            //        }
                            //    }
                            //}
                            DataSet dsFA = PhoenixCommonFileAttachment.AttachmentSOACheckingVoucherSearchForCombinedPDF(new Guid(DrVL["FLDDTKEY"].ToString()), null, null, null, null, 1, 100, ref iRowCount, ref iTotalPageCount);
                            if (dsFA.Tables.Count > 0)
                            {
                                DataTable DtFA = dsFA.Tables[0];
                                foreach (DataRow DrFA in DtFA.Rows) // Loop over the rows.
                                {

                                    strpath = "";
                                    strFilePath = "";
                                    strdirectoryname = "";

                                    strpath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + DrFA["FLDFILEPATH"].ToString();
                                    strFilePath = strpath.Substring(strpath.Length - 40, 40);

                                    strFilePath = DrVL["FLDVOUCHERLINEITEMNO"].ToString() + "_" + strFilePath;

                                    strdirectoryname = strpath.Substring(0, strpath.Length - 40);
                                    strdirectoryname = strdirectoryname + ViewState["debitnotereference"].ToString();

                                    strFilePath = strdirectoryname + "/" + strFilePath;

                                    if (!Directory.Exists(strdirectoryname))
                                        Directory.CreateDirectory(strdirectoryname);

                                    using (PdfSharp.Pdf.PdfDocument pdfDoc = PdfSharp.Pdf.IO.PdfReader.Open(PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + DrFA["FLDFILEPATH"].ToString(), PdfSharp.Pdf.IO.PdfDocumentOpenMode.Modify))
                                    {

                                        for (int i = 0; i < pdfDoc.PageCount; i++)
                                        {
                                            //// Variation 1: Draw a watermark as a text string.

                                            //// Get an XGraphics object for drawing beneath the existing content.
                                            var gfx = XGraphics.FromPdfPage(pdfDoc.Pages[i], XGraphicsPdfPageOptions.Append);


                                            XFont font = new XFont("Times New Roman", 12, XFontStyle.BoldItalic);

                                            // Create a string format.
                                            var format = new XStringFormat();
                                            format.Alignment = XStringAlignment.Near;
                                            format.LineAlignment = XLineAlignment.Near;

                                            // Create a dimmed red brush.
                                            XBrush brush = new XSolidBrush(XColor.FromArgb(128, 0, 0, 255));

                                            // Draw the string.

                                            if (DrFA["FLDATTACHMENTTYPE"].ToString() == "MST Acknowledgement" || DrFA["FLDATTACHMENTTYPE"].ToString() == "Owner Approval")
                                            {
                                                if (pdfDoc.Pages[i].Orientation.ToString() == "Landscape")
                                                {
                                                    gfx.DrawString((DrVL["FLDVOUCHERROW"].ToString()), font, brush, new XPoint(100, (pdfDoc.Pages[i].Height) - 40), format);
                                                }
                                                else
                                                {
                                                    gfx.DrawString((DrVL["FLDVOUCHERROW"].ToString()), font, brush, new XPoint(100, (pdfDoc.Pages[i].Height) - 50), format);
                                                }
                                            }
                                            else
                                            {
                                                if (pdfDoc.Pages[i].Orientation.ToString() == "Landscape")
                                                {

                                                    gfx.DrawString((DrVL["FLDDESCRIPTIONFORPRINT"].ToString()), font, brush, new XPoint(100, (pdfDoc.Pages[i].Height) - 40), format);
                                                    gfx.DrawString((DrVL["FLDREMARKS"].ToString()), font, brush, new XPoint(100, (pdfDoc.Pages[i].Height) - 30), format);
                                                }
                                                else
                                                {
                                                    gfx.DrawString((DrVL["FLDDESCRIPTIONFORPRINT"].ToString()), font, brush, new XPoint(100, (pdfDoc.Pages[i].Height) - 50), format);
                                                    gfx.DrawString((DrVL["FLDREMARKS"].ToString()), font, brush, new XPoint(100, (pdfDoc.Pages[i].Height) - 40), format);
                                                }
                                            }

                                            pdfDoc.Save(strFilePath);

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        if (ViewState["subreportcode"].ToString() == "ASLE")
        {
            DataTable DtBL = PhoenixAccountsSoaChecking.SoaCheckingESMBudgetList(int.Parse(ViewState["accountid"].ToString()), ViewState["debitnotereference"].ToString());

            if (DtBL.Rows.Count > 0)
            {
                foreach (DataRow DrBL in DtBL.Rows) // Loop over the rows.
                {
                    DataTable DtVL = PhoenixAccountsSoaChecking.SoaCheckingESMVouchersList(ViewState["debitnotereference"].ToString(), int.Parse(ViewState["Ownerid"].ToString()), General.GetNullableString(DrBL["FLDBUDGETID"].ToString()), int.Parse(ViewState["accountid"].ToString()));

                    if (DtVL.Rows.Count > 0)
                    {
                        string strpath, strFilePath, strdirectoryname;

                        int iRowCount = 0;
                        int iTotalPageCount = 0;

                        foreach (DataRow DrVL in DtVL.Rows) // Loop over the rows.
                        {

                            //DataSet dsVLFA = PhoenixCommonFileAttachment.AttachmentSOACheckingVoucherSearch(new Guid(DrVL["FLDVOUCHERDTKEY"].ToString()), null, null, null, null, 1, 100, ref iRowCount, ref iTotalPageCount);

                            //if (dsVLFA.Tables[0].Rows.Count > 0)
                            //{
                            //    DataTable DtVLFA = dsVLFA.Tables[0];
                            //    foreach (DataRow DrVLFA in DtVLFA.Rows) // Loop over the rows.
                            //    {

                            //        strpath = "";
                            //        strFilePath = "";
                            //        strdirectoryname = "";

                            //        strpath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + DrVLFA["FLDFILEPATH"].ToString();
                            //        strFilePath = strpath.Substring(strpath.Length - 40, 40);

                            //        strdirectoryname = strpath.Substring(0, strpath.Length - 40);
                            //        strdirectoryname = strdirectoryname + ViewState["debitnotereference"].ToString();

                            //        strFilePath = strdirectoryname + "/" + strFilePath;

                            //        if (!Directory.Exists(strdirectoryname))
                            //            Directory.CreateDirectory(strdirectoryname);

                            //        using (PdfSharp.Pdf.PdfDocument pdfDoc = PdfSharp.Pdf.IO.PdfReader.Open(PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + DrVLFA["FLDFILEPATH"].ToString(), PdfSharp.Pdf.IO.PdfDocumentOpenMode.Modify))
                            //        {

                            //            for (int i = 0; i < pdfDoc.PageCount; i++)
                            //            {
                            //                //// Variation 1: Draw a watermark as a text string.

                            //                //// Get an XGraphics object for drawing beneath the existing content.
                            //                var gfx = XGraphics.FromPdfPage(pdfDoc.Pages[i], XGraphicsPdfPageOptions.Append);

                            //                XFont font = new XFont("Times New Roman", 12, XFontStyle.BoldItalic);

                            //                // Create a string format.
                            //                var format = new XStringFormat();
                            //                format.Alignment = XStringAlignment.Near;
                            //                format.LineAlignment = XLineAlignment.Near;

                            //                // Create a dimmed red brush.
                            //                XBrush brush = new XSolidBrush(XColor.FromArgb(128, 0, 0, 255));

                            //                if (DrVLFA["FLDATTACHMENTTYPE"].ToString() == "MST Acknowledgement" || DrVLFA["FLDATTACHMENTTYPE"].ToString() == "Owner Approval")
                            //                {
                            //                    if (pdfDoc.Pages[i].Orientation.ToString() == "Landscape")
                            //                    {
                            //                        gfx.DrawString((DrVL["FLDVOUCHERROW"].ToString()), font, brush, new XPoint(100, (pdfDoc.Pages[i].Height) - 40), format);
                            //                    }
                            //                    else
                            //                    {
                            //                        gfx.DrawString((DrVL["FLDVOUCHERROW"].ToString()), font, brush, new XPoint(100, (pdfDoc.Pages[i].Height) - 50), format);
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    if (pdfDoc.Pages[i].Orientation.ToString() == "Landscape")
                            //                    {

                            //                        gfx.DrawString((DrVL["FLDDESCRIPTIONFORPRINT"].ToString()), font, brush, new XPoint(100, (pdfDoc.Pages[i].Height) - 40), format);
                            //                        gfx.DrawString((DrVL["FLDREMARKS"].ToString()), font, brush, new XPoint(100, (pdfDoc.Pages[i].Height) - 30), format);
                            //                    }
                            //                    else
                            //                    {
                            //                        gfx.DrawString((DrVL["FLDDESCRIPTIONFORPRINT"].ToString()), font, brush, new XPoint(100, (pdfDoc.Pages[i].Height) - 50), format);
                            //                        gfx.DrawString((DrVL["FLDREMARKS"].ToString()), font, brush, new XPoint(100, (pdfDoc.Pages[i].Height) - 40), format);
                            //                    }
                            //                }

                            //                pdfDoc.Save(strFilePath);
                            //            }
                            //        }
                            //    }
                            //}
                            DataSet dsFA = PhoenixCommonFileAttachment.AttachmentSOACheckingVoucherSearchForCombinedPDF(new Guid(DrVL["FLDDTKEY"].ToString()), null, null, null, null, 1, 100, ref iRowCount, ref iTotalPageCount);
                            if (dsFA.Tables.Count > 0)
                            {
                                DataTable DtFA = dsFA.Tables[0];
                                foreach (DataRow DrFA in DtFA.Rows) // Loop over the rows.
                                {

                                    strpath = "";
                                    strFilePath = "";
                                    strdirectoryname = "";
                                    strpath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + DrFA["FLDFILEPATH"].ToString();
                                    strFilePath = strpath.Substring(strpath.Length - 40, 40);

                                    strFilePath = DrVL["FLDVOUCHERLINEITEMNO"].ToString() + "_" + strFilePath;

                                    strdirectoryname = strpath.Substring(0, strpath.Length - 40);
                                    strdirectoryname = strdirectoryname + ViewState["debitnotereference"].ToString();

                                    strFilePath = strdirectoryname + "/" + strFilePath;

                                    if (!Directory.Exists(strdirectoryname))
                                        Directory.CreateDirectory(strdirectoryname);
                                    using (PdfSharp.Pdf.PdfDocument pdfDoc = PdfSharp.Pdf.IO.PdfReader.Open(PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + DrFA["FLDFILEPATH"].ToString(), PdfSharp.Pdf.IO.PdfDocumentOpenMode.Modify))
                                    {

                                        for (int i = 0; i < pdfDoc.PageCount; i++)
                                        {
                                            //// Variation 1: Draw a watermark as a text string.

                                            //// Get an XGraphics object for drawing beneath the existing content.
                                            var gfx = XGraphics.FromPdfPage(pdfDoc.Pages[i], XGraphicsPdfPageOptions.Append);

                                            XFont font = new XFont("Times New Roman", 12, XFontStyle.BoldItalic);

                                            // Create a string format.
                                            var format = new XStringFormat();
                                            format.Alignment = XStringAlignment.Near;
                                            format.LineAlignment = XLineAlignment.Near;

                                            // Create a dimmed red brush.
                                            XBrush brush = new XSolidBrush(XColor.FromArgb(128, 0, 0, 255));

                                            // Draw the string.
                                            if (DrFA["FLDATTACHMENTTYPE"].ToString() == "MST Acknowledgement" || DrFA["FLDATTACHMENTTYPE"].ToString() == "Owner Approval")
                                            {
                                                if (pdfDoc.Pages[i].Orientation.ToString() == "Landscape")
                                                {
                                                    gfx.DrawString((DrVL["FLDVOUCHERROW"].ToString()), font, brush, new XPoint(100, (pdfDoc.Pages[i].Height) - 40), format);
                                                }
                                                else
                                                {
                                                    gfx.DrawString((DrVL["FLDVOUCHERROW"].ToString()), font, brush, new XPoint(100, (pdfDoc.Pages[i].Height) - 50), format);
                                                }
                                            }
                                            else
                                            {
                                                if (pdfDoc.Pages[i].Orientation.ToString() == "Landscape")
                                                {

                                                    gfx.DrawString((DrVL["FLDDESCRIPTIONFORPRINT"].ToString()), font, brush, new XPoint(100, (pdfDoc.Pages[i].Height) - 40), format);
                                                    gfx.DrawString((DrVL["FLDREMARKS"].ToString()), font, brush, new XPoint(100, (pdfDoc.Pages[i].Height) - 30), format);
                                                }
                                                else
                                                {
                                                    gfx.DrawString((DrVL["FLDDESCRIPTIONFORPRINT"].ToString()), font, brush, new XPoint(100, (pdfDoc.Pages[i].Height) - 50), format);
                                                    gfx.DrawString((DrVL["FLDREMARKS"].ToString()), font, brush, new XPoint(100, (pdfDoc.Pages[i].Height) - 40), format);
                                                }
                                            }

                                            pdfDoc.Save(strFilePath);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

}
