using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;


public partial class AccountsSOAGenerationSubledgerType : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            confirm.Attributes.Add("style", "display:none");
            confirmno.Attributes.Add("style", "display:none");
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

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Generate", "GENERATE", ToolBarDirection.Right);
        MenuCommentsEdit.AccessRights = this.ViewState;
        MenuCommentsEdit.MenuList = toolbarmain.Show();

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
                        ucConfirmMsg.RadConfirm("PDF was already generated earlier and whether they would need a new file to be generated will be displayed. This file will replace previous version of the file", "confirm", 320, 150, null, "Confirm");
                        //ucConfirmMsg.Visible = true;
                        //ucConfirmMsg.Text = "PDF was already generated earlier and whether they would need a new file to be generated will be displayed. This file will replace previous version of the file";
                        return;
                    }
                    else
                    {
                        Response.Redirect("../Accounts/AccountsSOAGenerationPdfReport.aspx?pdfGen=1&debitnotereference=" + ViewState["debitnotereference"] + "&debitnotereferenceid=" + ViewState["debitnotereferenceid"] + "&accountid="
                                           + ViewState["accountid"] + "&Ownerid=" + ViewState["Ownerid"] + "&dtkey=" + ViewState["dtkey"] + "&description=" + ViewState["description"] + "&year=" + ViewState["year"] + "&month=" + ViewState["month"] + "&Type=0&URL=" + ViewState["URL"]);
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

    protected void CheckMapping_Click(object sender, EventArgs e)
    {
        try
        {
            //UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;
            //if (ucConfirmMsg.)
            //{
                Response.Redirect("../Accounts/AccountsSOAGenerationPdfReport.aspx?pdfGen=1&debitnotereference=" + ViewState["debitnotereference"] + "&debitnotereferenceid=" + ViewState["debitnotereferenceid"] + "&accountid="
                                          + ViewState["accountid"] + "&Ownerid=" + ViewState["Ownerid"] + "&dtkey=" + ViewState["dtkey"] + "&description=" + ViewState["description"] + "&year=" + ViewState["year"] + "&month=" + ViewState["month"] + "&Type=0&URL=" + ViewState["URL"]);
            //}
            //else
            //{
            //    Response.Redirect("../Accounts/AccountsSOAGenerationPdfReport.aspx?pdfGen=0&debitnotereference=" + ViewState["debitnotereference"] + "&debitnotereferenceid=" + ViewState["debitnotereferenceid"] + "&accountid="
            //                              + ViewState["accountid"] + "&Ownerid=" + ViewState["Ownerid"] + "&dtkey=" + ViewState["dtkey"] + "&description=" + ViewState["description"] + "&year=" + ViewState["year"] + "&month=" + ViewState["month"] + "&Type=0&URL=" + ViewState["URL"]);
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }




    protected void confirmno_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Accounts/AccountsSOAGenerationPdfReport.aspx?pdfGen=0&debitnotereference=" + ViewState["debitnotereference"] + "&debitnotereferenceid=" + ViewState["debitnotereferenceid"] + "&accountid="
                                                 + ViewState["accountid"] + "&Ownerid=" + ViewState["Ownerid"] + "&dtkey=" + ViewState["dtkey"] + "&description=" + ViewState["description"] + "&year=" + ViewState["year"] + "&month=" + ViewState["month"] + "&Type=0&URL=" + ViewState["URL"]);
    }
}
