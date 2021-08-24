using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;


public partial class AccountsSoaCheckingSubledgerType : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Generate", "GENERATE",ToolBarDirection.Right);
        MenuCommentsEdit.AccessRights = this.ViewState;
        MenuCommentsEdit.Title = "SOA Checking Sub Ledger Type";
        MenuCommentsEdit.MenuList = toolbarmain.Show();


        if (!IsPostBack)
        {

            if (!IsPostBack)
            {
                ViewState["debitnotereference"] = Request.QueryString["debitnotereference"].ToString();
                ViewState["debitnotereferenceid"] = Request.QueryString["debitnotereferenceid"].ToString();
                ViewState["accountid"] = Request.QueryString["accountid"].ToString();
                ViewState["Ownerid"] = Request.QueryString["Ownerid"].ToString();
                ViewState["dtkey"] = Request.QueryString["dtkey"].ToString();
                ViewState["description"] = Request.QueryString["description"].ToString();
                ViewState["year"] = Request.QueryString["year"].ToString();
                ViewState["month"] = Request.QueryString["month"].ToString();
                ViewState["pdfGen"] = Request.QueryString["pdfGen"].ToString();
                ViewState["VOUCHERDTKEY"] = "";
                ViewState["URL"] = Request.QueryString["URL"].ToString();
            }

                //ucCommentsDueDate.Enabled = false;
                //ucCommentsDueDate.ReadOnly = true;
            if (rblbtn1.SelectedValue == "0")
            {
                //ucCommentsDueDate.Enabled = false;
                //ucCommentsDueDate.ReadOnly = true;
            }


            //BindData();
        }
    }

    protected void MenuCommentsEdit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("GENERATE"))
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
                        ucConfirmMsg.Visible = true;
                        ucConfirmMsg.Text = "PDF was already generated earlier and whether they would need a new file to be generated will be displayed. This file will replace previous version of the file";
                        return;
                    }
                    else
                    {
                        Response.Redirect("../Accounts/AccountsSOAGenerationPdfReport.aspx?pdfGen=1&debitnotereference=" + ViewState["debitnotereference"] + "&debitnotereferenceid=" + ViewState["debitnotereferenceid"] + "&accountid="
                                           + ViewState["accountid"] + "&Ownerid=" + ViewState["Ownerid"] + "&dtkey=" + ViewState["dtkey"] + "&description=" + ViewState["description"] + "&year=" + ViewState["year"] + "&month=" + ViewState["month"] + "&Type=0&URL=" + ViewState["URL"]);
                    }
                }
                //if (rblbtn1.SelectedValue == "0")
                //{

                //    Response.Redirect("../Accounts/AccountsSoaCheckingPdfReport.aspx?pdfGen=1&debitnotereference=" + ViewState["debitnotereference"] + "&debitnotereferenceid=" + ViewState["debitnotereferenceid"] + "&accountid="
                //                    + ViewState["accountid"] + "&Ownerid=" + ViewState["Ownerid"] + "&dtkey=" + ViewState["dtkey"] + "&description=" + ViewState["description"] + "&year=" + ViewState["year"] + "&month=" + ViewState["month"] + "&Type=0");

                //}
                //if (rblbtn1.SelectedValue == "1")
                //{

                //    Response.Redirect("../Accounts/AccountsSoaCheckingESMBudgetCodePdfReport.aspx?pdfGen=1&debitnotereference=" + ViewState["debitnotereference"] + "&debitnotereferenceid=" + ViewState["debitnotereferenceid"] + "&accountid="
                //                    + ViewState["accountid"] + "&Ownerid=" + ViewState["Ownerid"] + "&dtkey=" + ViewState["dtkey"] + "&description=" + ViewState["description"] + "&year=" + ViewState["year"] + "&month=" + ViewState["month"] + "&Type=1");

                //}

            }            

            //String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
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
            UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;
            if (ucCM.confirmboxvalue == 1)
            {
                Response.Redirect("../Accounts/AccountsSOAGenerationPdfReport.aspx?pdfGen=1&debitnotereference=" + ViewState["debitnotereference"] + "&debitnotereferenceid=" + ViewState["debitnotereferenceid"] + "&accountid="
                                          + ViewState["accountid"] + "&Ownerid=" + ViewState["Ownerid"] + "&dtkey=" + ViewState["dtkey"] + "&description=" + ViewState["description"] + "&year=" + ViewState["year"] + "&month=" + ViewState["month"] + "&Type=0&URL=" + ViewState["URL"]);
            }
            else
            {
                Response.Redirect("../Accounts/AccountsSOAGenerationPdfReport.aspx?pdfGen=0&debitnotereference=" + ViewState["debitnotereference"] + "&debitnotereferenceid=" + ViewState["debitnotereferenceid"] + "&accountid="
                                          + ViewState["accountid"] + "&Ownerid=" + ViewState["Ownerid"] + "&dtkey=" + ViewState["dtkey"] + "&description=" + ViewState["description"] + "&year=" + ViewState["year"] + "&month=" + ViewState["month"] + "&Type=0&URL=" + ViewState["URL"]);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //private void BindData()
    //{
    //    string strrblbtn1 = "";
    //    DataSet ds = PhoenixDocumentManagementDocument.EditDMSComments(new Guid(ViewState["COMMENTID"].ToString()));
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        DataRow dr = ds.Tables[0].Rows[0];
    //        txtOfficeRemarks.Text = dr["FLDOFFICEREMARKS"].ToString();
    //        ucCommentsDueDate.Text = dr["FLDDUEDATE"].ToString();
    //        if (dr["FLDACCEPTEDDOCUMENTYN"].ToString() == "1")
    //        {
    //            strrblbtn1 = "1";
    //            rblbtn1.SelectedValue = strrblbtn1;
    //        }
    //        else
    //        {
    //            strrblbtn1 = "0";
    //            rblbtn1.SelectedValue = strrblbtn1;
    //        }
    //    }
    //}
}
