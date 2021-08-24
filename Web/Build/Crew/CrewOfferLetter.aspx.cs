using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.VesselAccounts;
using System.IO;
using Telerik.Web.UI;
public partial class CrewOfferLetter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            ViewState["Offerletterid"] = "";
            SetEmployeePrimaryDetails();
            cmdHiddenPick.Attributes.Add("style", "display:none;");
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
        }
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Back", "BACK", ToolBarDirection.Right);
        toolbarmain.AddImageLink("javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=OFFERLETTERCHECKLIST&offerletter=" + ViewState["Offerletterid"].ToString() + "&fileno=" + txtEmployeeNumber.Text + "&showmenu=1'); return false;", "Report", "", "REPORT", ToolBarDirection.Right);
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);

        CrewOfferLetterTabs.AccessRights = this.ViewState;
        CrewOfferLetterTabs.MenuList = toolbarmain.Show();

    }
    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataSet ds = PhoenixCrewOfferLetter.OfferletterEdit(new Guid(Request.QueryString["id"].ToString()));
            DataRow dr = ds.Tables[0].Rows[0];
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["Offerletterid"] = dr["FLDID"].ToString();
                txtEmployeeNumber.Text = dr["FLDFILENO"].ToString();
                ViewState["empid"] = dr["FLDEMPLOYEEID"].ToString();
                txtEmployeeName.Text = dr["FLDEMPLOYEENAME"].ToString();
                txtRank.Text = dr["FLDRANKCODE"].ToString();
                ViewState["RANKID"] = dr["FLDRANK"].ToString();
                ddlVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
                ddlUnion.SelectedAddress = dr["FLDCBA"].ToString();
                ucCategory.SelectedHard = dr["FLDVESSELTYPE"].ToString();
                //ucVesselType.SelectedVesseltype = dr["FLDVESSELTYPE"].ToString();
                Ucsupt.SelectedValue = dr["FLDSUPTID"].ToString();
                Ucsupt.Text = dr["FLDSUPTNAMEDESC"].ToString();
                txtAccountno.Text = dr["FLDACCOUNTNUMBER"].ToString();
                txtContractPeriod.Text = dr["FLDCONTRACTPERIOD"].ToString();
                txtcoursestodo.Text = dr["FLDCOURSESTODO"].ToString();
                txtDOA.Text = dr["FLDDOAFORCOURSES"].ToString();
                txtdocrecived.Text = dr["FLDDOCUMENTRECIEVED"].ToString();
                txtdocumentationmention.Text = dr["FLDDOCUMENTATIONMENTION"].ToString();
                txtnoofdays.Text = dr["FLDNOOFDAYS"].ToString();
                txtPlusMinusPeriod.Text = dr["FLDPLUSORMINUS"].ToString();
                txtSalAgreed.Text = dr["FLDWAGESAGREED"].ToString();
                txtsuptdate.Text = dr["FLDSUPTDATE"].ToString();
                txtTraveldate.Text = dr["FLDTRAVELREADINESS"].ToString();
                txtlastwages.Text = dr["FLDLASTWAGEDRAWN"].ToString();
                txtsupt.Text = dr["FLDSUPTNAME"].ToString();
                txtanyothercommitment.Text = dr["FLDANYOTHERCOMMIMENTS"].ToString();
                rdbisbank.SelectedValue = dr["FLDISACCOUNTOPENED"].ToString();
                rdbisbriefed.SelectedValue = dr["FLDISBRIEFEDCOURSES"].ToString();
                rdboc9e.SelectedValue = dr["FLDISOC9E"].ToString();
                rdbnewkyc.SelectedValue = dr["FLDNEWKYC"].ToString();
                rdbissutiable.SelectedValue = dr["FLDISCANDIDATESUTIABLE"].ToString();
                lbladdtionalwages.Text = dr["FLDADDITIONALWAGES"].ToString();//
                imgBtncrewagreed.Attributes.Add("onclick", "return parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewOfferLetterWages.aspx?offerid=" + Request.QueryString["id"].ToString() + "');return false;");

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void GvOfferLetter_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

        }


    }
    protected void GvOfferLetter_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : GvOfferLetter.CurrentPageIndex + 1;
        BindData();
    }
    protected void GvOfferLetter_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("ADD"))
            {

            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void GvOfferLetter_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {

    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        SetEmployeePrimaryDetails();
    }
    protected void CrewOfferLetterTabs_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {

                if (!IsValidComponent())
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewOfferLetter.CrewOfferletterUpdate(int.Parse(ViewState["empid"].ToString()), int.Parse(ViewState["RANKID"].ToString()), null, General.GetNullableInteger(ddlVessel.SelectedVessel), General.GetNullableInteger(ucCategory.SelectedHard), General.GetNullableDecimal(txtlastwages.Text), General.GetNullableDecimal(txtSalAgreed.Text)
                    , General.GetNullableDateTime(txtDOA.Text), General.GetNullableDateTime(txtTraveldate.Text), General.GetNullableInteger(txtnoofdays.Text), General.GetNullableInteger(rdbisbriefed.SelectedValue), General.GetNullableInteger(ddlUnion.SelectedAddress), General.GetNullableString(txtcoursestodo.Text)
                    , General.GetNullableInteger(rdbisbank.SelectedValue), General.GetNullableString(txtAccountno.Text), General.GetNullableString(txtanyothercommitment.Text), General.GetNullableString(txtsupt.Text), General.GetNullableInteger(Ucsupt.SelectedValue), General.GetNullableDateTime(txtsuptdate.Text), General.GetNullableString(txtdocrecived.Text)
                    , General.GetNullableInteger(rdbissutiable.SelectedValue), General.GetNullableInteger(txtContractPeriod.Text), General.GetNullableInteger(txtPlusMinusPeriod.Text), new Guid(Request.QueryString["id"].ToString()), General.GetNullableInteger(rdboc9e.SelectedValue), General.GetNullableInteger(rdbnewkyc.SelectedValue), General.GetNullableString(txtdocumentationmention.Text));
                SetEmployeePrimaryDetails();
            }
            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Crew/CrewOfferLetterList.aspx?Type="+ Request.QueryString["Type"].ToString() +"&empid=" + Request.QueryString["empid"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidComponent()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        
        if (General.GetNullableInteger(ucCategory.SelectedHard) == null)
            ucError.ErrorMessage = "Vessel Type is required";
        return (!ucError.IsError);
    }
    protected void CrewAppraisal_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindData()
    {
        try
        {
            DataSet ds = PhoenixCrewOfferLetter.OfferletterQuestionmappedEdit(new Guid(Request.QueryString["id"].ToString()));
            GvOfferLetter.DataSource = ds;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
