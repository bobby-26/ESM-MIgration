using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;

public partial class InspectionReportReceived : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "UPDATE");
        MenuReport.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["DEFICIENCYID"] = "";
            ViewState["TYPE"] = "";
            if (Request.QueryString["DEFICIENCYID"] != null && Request.QueryString["DEFICIENCYID"].ToString() != string.Empty)
            {
                ViewState["DEFICIENCYID"] = Request.QueryString["DEFICIENCYID"].ToString();
            }
            EditDeficiency();
        }
    }
    protected void EditDeficiency()
    {
        if (ViewState["DEFICIENCYID"] != null && ViewState["DEFICIENCYID"].ToString() != string.Empty)
        {
            DataSet ds = PhoenixInspectionAuditNonConformity.EditAuditNC(new Guid(ViewState["DEFICIENCYID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtReferenceNumber.Text = dr["FLDNONCONFORMITYNUMBER"].ToString();
                txtCompletionDate.Text = dr["FLDNCCOMPLETIONDATE"].ToString();
                ViewState["TYPE"] = 1;
            }

            DataSet ds1 = PhoenixInspectionObservation.EditDirectObservation(new Guid(ViewState["DEFICIENCYID"].ToString()));
            if (ds1.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds1.Tables[0].Rows[0];
                txtReferenceNumber.Text = dr["FLDOBSERVATIONNUMBER"].ToString();
                txtCompletionDate.Text = dr["FLDCOMPLETIONDATE"].ToString();
                ViewState["TYPE"] = 2;
            }
        }
    }

    protected void MenuReport_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;


            if (IsValidDeficiency())
            {
                string Script = "";

                if (ViewState["DEFICIENCYID"] != null && ViewState["DEFICIENCYID"].ToString() != string.Empty)
                {
                    if (dce.CommandName.ToUpper().Equals("UPDATE"))
                    {
                        if (ViewState["TYPE"] != null && ViewState["TYPE"].ToString().Equals("1")) //nc
                        {
                            PhoenixInspectionDeficiency.NCDeficiencyRCAComplete(new Guid(ViewState["DEFICIENCYID"].ToString()),
                                DateTime.Parse(txtCompletionDate.Text));
                        }
                        if (ViewState["TYPE"] != null && ViewState["TYPE"].ToString().Equals("2")) //obs
                        {
                            PhoenixInspectionDeficiency.ObsDeficiencyRCAComplete(new Guid(ViewState["DEFICIENCYID"].ToString()),
                                DateTime.Parse(txtCompletionDate.Text));
                        }

                        EditDeficiency();
                        //ucStatus.Text = "Deficiency is completed.";

                        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                        Script += "fnReloadList('codehelp1','ifMoreInfo',null);";
                        Script += "</script>" + "\n";
                        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                        lblMessage.Text = "";
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

    private bool IsValidDeficiency()
    {
        if (General.GetNullableDateTime(txtCompletionDate.Text) == null)
        {
            ucError.ErrorMessage = "RCA Completion Date is required.";
        }
        else if (General.GetNullableDateTime(txtCompletionDate.Text) > DateTime.Today)
        {
            ucError.ErrorMessage = "RCA Completion Date cannot be the future date.";
            EditDeficiency();
        }
        ucError.Visible = true;
        return (!ucError.IsError);


    }
}
