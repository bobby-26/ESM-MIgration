using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class InspectionRAImpactDocumentPPEMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbartab = new PhoenixToolbar();
            toolbartab.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuMapping.AccessRights = this.ViewState;
            MenuMapping.MenuList = toolbartab.Show();

            if (!IsPostBack)
            {
                ViewState["SUBHAZARDID"] = "";
                if ((Request.QueryString["subhazardid"] != null) && (Request.QueryString["subhazardid"] != ""))
                {
                    ViewState["SUBHAZARDID"] = Request.QueryString["subhazardid"].ToString();
                }

                ViewState["TYPE"] = "0";
                if ((Request.QueryString["type"] != null) && (Request.QueryString["type"] != ""))
                {
                    ViewState["TYPE"] = Request.QueryString["type"];
                    ucCategory.Type = Request.QueryString["type"];

                    if ((Request.QueryString["type"].ToString().Equals("2")) || (Request.QueryString["type"].ToString().Equals("4")))
                    {
                        lblPPE.Visible = false;
                        cblRecomendedPPE.Visible = false;
                    }
                }

                ViewState["COMPANYID"] = "0";

                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                    ViewState["COMPANYID"] = nvc.Get("QMS");

                if (ViewState["TYPE"].ToString() == "4")
                {
                    chkimpacts.Visible = true;
                    ddlImpact.Visible = false;
                }
                else
                {
                    chkimpacts.Visible = false;
                    ddlImpact.Visible = true;
                }


                BindContact();
                BindImpact();
                BindRecomendedPPE();
                DetailEdit();
                BindHazard();            
            }

            if(ViewState["TYPE"].ToString() =="4")
            {
                hazard.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void BindContact()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionContractTypeExtn.ListContactType();

        DataTable dt = ds.Tables[0];

        chkundesirableevent.Items.Clear();
        chkundesirableevent.DataSource = dt;
        chkundesirableevent.DataBind();
    }

    protected void BindImpact()
    {
        DataTable dt = new DataTable();
        dt = PhoenixInspectionOperationalRiskControls.ListRiskAssessmentImpact(General.GetNullableInteger(ViewState["TYPE"].ToString()));

        if (dt.Rows.Count > 0)
        {
            ddlImpact.Items.Clear();
            ddlImpact.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            ddlImpact.DataSource = dt;
            ddlImpact.DataBind();

            chkimpacts.ClearSelection();
            chkimpacts.DataSource = dt;
            chkimpacts.DataBind();
        }
    }

    private string GetCsvValue(RadComboBox radComboBox)
    {
        var list = radComboBox.CheckedItems;
        string csv = string.Empty;
        if (list.Count != 0)
        {
            csv = ",";
            foreach (var item in list)
            {
                csv = csv + item.Value + ",";
            }
        }
        return csv;
    }
    private void SetCsvValue(RadComboBox radComboBox, string csvValue)
    {
        foreach (RadComboBoxItem item in radComboBox.Items)
        {
            item.Checked = false;
            if (csvValue.Contains("," + item.Value + ","))
            {
                item.Checked = true;
            }
        }
    }

    protected void MenuMapping_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;

            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string recommendedppe = General.ReadCheckBoxList(cblRecomendedPPE);

                if (!IsValidRASubHazard())
                {
                    ucError.Visible = true;
                    return;
                }

                if (ViewState["SUBHAZARDID"].ToString().Equals(""))
                {
                    Guid? subhazardid = General.GetNullableGuid(null);

                    PhoenixInspectionRiskAssessmentSubHazardExtn.InsertRiskAssessmentSubHazardNew(ref subhazardid
                    , General.GetNullableInteger(ucCategory.SelectedHazardType)
                    , General.GetNullableString(txtSubHazard.Text.Trim())
                    , General.GetNullableGuid(ddlImpact.SelectedValue)
                    , General.GetNullableString(General.ReadCheckBoxList(chkundesirableevent))
                    , General.GetNullableString(recommendedppe)
                    , General.GetNullableString(General.ReadCheckBoxList(chkworstcase))
                    , General.GetNullableString(General.ReadCheckBoxList(chkimpacts))
                    , General.GetNullableGuid(ddlHazard.SelectedValue));

                    ViewState["SUBHAZARDID"] = subhazardid;
                }
                else
                {
                    PhoenixInspectionRiskAssessmentSubHazardExtn.UpdateRiskAssessmentSubHazardNew(new Guid(ViewState["SUBHAZARDID"].ToString())
                    , General.GetNullableInteger(ucCategory.SelectedHazardType)
                    , General.GetNullableString(txtSubHazard.Text.Trim())
                    , General.GetNullableGuid(ddlImpact.SelectedValue)
                    , General.GetNullableString(General.ReadCheckBoxList(chkundesirableevent))
                    , General.GetNullableString(recommendedppe)
                    , General.GetNullableString(General.ReadCheckBoxList(chkworstcase))
                    , General.GetNullableString(General.ReadCheckBoxList(chkimpacts))
                    , General.GetNullableGuid(ddlHazard.SelectedValue));
                }

                DetailEdit();
                ucStatus.Text = "Information updated.";

                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo',null);";
                Script += "</script>" + "\n";
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidRASubHazard()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        string recommendedppe = General.ReadCheckBoxList(cblRecomendedPPE);

        if (General.GetNullableInteger(ucCategory.SelectedHazardType) == null)
            ucError.ErrorMessage = "Category is required.";

        if (General.GetNullableString(txtSubHazard.Text.Trim()) == null)
            ucError.ErrorMessage = "Hazard is required.";

        if (ViewState["TYPE"].ToString() != "4")
        {
            if (General.GetNullableGuid(ddlImpact.SelectedValue) == null)
                ucError.ErrorMessage = "Impact is required.";
        }
        else
        {
            if (General.GetNullableString(General.ReadCheckBoxList(chkimpacts)) == null)
                ucError.ErrorMessage = "Impact is required.";
        }

        if (General.GetNullableString(General.ReadCheckBoxList(chkundesirableevent)) == null)
            ucError.ErrorMessage = "Contact Type / Undesirable Event is required.";

        if (General.GetNullableString(General.ReadCheckBoxList(chkworstcase)) == null)
            ucError.ErrorMessage = "Worst Case is required.";

        if (ViewState["TYPE"].ToString() == "1")
        {
            if (General.GetNullableString(recommendedppe) == null)
                ucError.ErrorMessage = "PPE is required.";
        }

        return (!ucError.IsError);
    }

    protected void BindRecomendedPPE()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        ds = PhoenixInspectionRiskAssessmentMiscellaneousExtn.RiskAssessmentMiscellaneousSearch(null,
            5,
            null, null,
            1,
            500,
            ref iRowCount,
            ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            cblRecomendedPPE.DataSource = ds.Tables[0];
            cblRecomendedPPE.DataBind();
        }
    }
    private void DetailEdit()
    {
        if (ViewState["SUBHAZARDID"].ToString() != "")
        {
            DataSet ds = PhoenixInspectionRiskAssessmentSubHazardExtn.EditRiskAssessmentSubHazard(new Guid(ViewState["SUBHAZARDID"].ToString()));
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                ucCategory.SelectedHazardType = dt.Rows[0]["FLDHAZARDID"].ToString();
                txtSubHazard.Text = dt.Rows[0]["FLDNAME"].ToString();
                General.BindCheckBoxList(cblRecomendedPPE, dt.Rows[0]["FLDRECOMMENDEDPPE"].ToString());                
                General.BindCheckBoxList(chkundesirableevent, dt.Rows[0]["FLDUNDISIRABLEEVENTID"].ToString());
                ViewState["TYPE"] = dt.Rows[0]["FLDHAZARDTYPEID"].ToString();
                ucCategory.Type = dt.Rows[0]["FLDHAZARDTYPEID"].ToString();
                if ((dt.Rows[0]["FLDHAZARDTYPEID"].ToString().Equals("2")) || (dt.Rows[0]["FLDHAZARDTYPEID"].ToString().Equals("4")))
                {
                    lblPPE.Visible = false;
                    cblRecomendedPPE.Visible = false;
                }

                if (dt.Rows[0]["FLDHAZARDTYPEID"].ToString().Equals("4"))
                {
                    chkimpacts.Visible = true;
                    ddlImpact.Visible = false;
                }
                else
                {
                    chkimpacts.Visible = false;
                    ddlImpact.Visible = true;
                }

                BindContactTypeWorstCase();
                BindImpact();
                ddlImpact.SelectedValue = dt.Rows[0]["FLDIMPACTID"].ToString();
                ddlHazard.SelectedValue = dt.Rows[0]["FLDIMMEDIATECAUSEID"].ToString();
                General.BindCheckBoxList(chkworstcase, dt.Rows[0]["FLDWORSTCASEIDS"].ToString());
                General.BindCheckBoxList(chkimpacts, dt.Rows[0]["FLDIMPACTIDS"].ToString());             
            }
        }
    }

    protected void BindContactTypeWorstCase()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionRiskAssessmentHazardExtn.ListContactTypeHazard(General.GetNullableString(General.ReadCheckBoxList(chkundesirableevent)));

        DataTable dt = ds.Tables[0];
        chkworstcase.Items.Clear();
        chkworstcase.DataSource = dt;
        chkworstcase.DataBind();
    }
    protected void BindHazard()
    {
        DataTable dt = PhoenixInspectionImmediateCause.ImmediateCausesbyCategoryList(General.GetNullableInteger(null));
        ddlHazard.DataSource = dt;
        ddlHazard.DataTextField = "FLDIMMEDIATECAUSE";
        ddlHazard.DataValueField = "FLDIMMEDIATECAUSEID";
        ddlHazard.DataBind();
        ddlHazard.Items.Insert(0, "--Select--");

    }
    protected void chkundesirableevent_SelectedIndexChanged(object sender, EventArgs e)
    {
        string worstcaselist = General.ReadCheckBoxList(chkworstcase);
        BindContactTypeWorstCase();
        General.BindCheckBoxList(chkworstcase, worstcaselist);
    }
    protected void ddlHazard_SelectedIndexChanged1(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        txtSubHazard.Text = ddlHazard.SelectedItem.Text;
    }
}
