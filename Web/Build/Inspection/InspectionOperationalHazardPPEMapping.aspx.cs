using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class InspectionOperationalHazardPPEMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbartab = new PhoenixToolbar();
            toolbartab.AddButton("List", "LIST", ToolBarDirection.Right);
            toolbartab.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuMapping.AccessRights = this.ViewState;
            MenuMapping.MenuList = toolbartab.Show();

            if (!IsPostBack)
            {

                ViewState["OPERATIONALCATEGORYID"] = "";
                if ((Request.QueryString["OperationalCategoryid"] != null) && (Request.QueryString["OperationalCategoryid"] != ""))
                {
                    ViewState["OPERATIONALCATEGORYID"] = Request.QueryString["OPERATIONALCATEGORYID"].ToString();
                }

                ViewState["COMPANYID"] = "0";

                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                    ViewState["COMPANYID"] = nvc.Get("QMS");

                int companyid = int.Parse(ViewState["COMPANYID"].ToString());

                imgComponent.Attributes.Add("onclick", "return showPickList('spnPickListComponent', 'codehelp1', '', '../Common/CommonPickListGlobalComponent.aspx'); ");

                BindRecomendedPPE();
                ProcedureDetailEdit();
            }
            BindComponents();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void lnkComponentAdd_Click(object sender, EventArgs e)
    {
        if (General.GetNullableGuid(txtComponentId.Text) != null)
        {

            PhoenixInspectionOperationalRiskControls.OperationalHazardComponents(new Guid(ViewState["OPERATIONALCATEGORYID"].ToString()), new Guid(txtComponentId.Text));
            ucStatus.Text = "Component added.";
            txtComponentId.Text = "";
            txtComponentCode.Text = "";
            txtComponentName.Text = "";
            BindComponents();
        }
    }

    protected void BindComponents()
    {
        DataSet dss = PhoenixInspectionOperationalRiskControls.OperationalHealthHazardComponentList(ViewState["OPERATIONALCATEGORYID"] == null ? null : General.GetNullableGuid(ViewState["OPERATIONALCATEGORYID"].ToString()));
        int number = 1;
        if (dss.Tables[0].Rows.Count > 0)
        {
            tblForms.Rows.Clear();
            foreach (DataRow dr in dss.Tables[0].Rows)
            {
                RadCheckBox cb = new RadCheckBox();
                cb.ID = dr["FLDCOMPONENTID"].ToString();
                cb.Text = "";
                cb.Checked = true;
                cb.AutoPostBack = true;
                cb.CheckedChanged += new EventHandler(cb_CheckedChanged);
                HyperLink hl = new HyperLink();
                hl.Text = dr["FLDCOMPONENTNAME"].ToString();
                hl.ID = "hlink" + number.ToString();
                hl.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");
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

    void cb_CheckedChanged(object sender, EventArgs e)
    {
        RadCheckBox c = (RadCheckBox)sender;
        if (c.Checked == false)
        {
            PhoenixInspectionOperationalRiskControls.OperationalHazardComponentDelete(new Guid(ViewState["OPERATIONALCATEGORYID"].ToString()), new Guid(c.ID));

            string txt = "";

            ucStatus.Text = txt + "deleted.";
            BindComponents();
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
                string recommendedppe = ReadCheckBoxList(cblRecomendedPPE);

                if (!IsValidRASubHazard())
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionOperationalRiskControls.UpdateOperationalHazardCategory(General.GetNullableGuid(ViewState["OPERATIONALCATEGORYID"].ToString())
                    ,  General.GetNullableString(txtSubHazard.Text)
                    ,  General.GetNullableString(txtoperational.Text)
                   , General.GetNullableString(recommendedppe));

                ProcedureDetailEdit();
                ucStatus.Text = "Information updated.";
            }

            if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../Inspection/InspectionOperationalRiskControlMapping.aspx?OperationalHazardid=" + ViewState["OPERATIONALHAZARDID"], false);
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

        if (General.GetNullableString(txtSubHazard.Text.Trim()) == null)
            ucError.ErrorMessage = "Hazard is required.";

        if (General.GetNullableString(txtoperational.Text.Trim()) == null)
            ucError.ErrorMessage = "Operational is required.";

        return (!ucError.IsError);
    }

    protected void BindRecomendedPPE()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        ds = PhoenixInspectionRiskAssessmentMiscellaneous.RiskAssessmentMiscellaneousSearch(null,
            5,
            null, null,
            1,
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            cblRecomendedPPE.DataSource = ds.Tables[0];
            cblRecomendedPPE.DataBind();
        }
    }
    private void ProcedureDetailEdit()
    {
        DataSet ds = PhoenixInspectionOperationalRiskControls.EditOpearionalCategoryMapping(General.GetNullableGuid(ViewState["OPERATIONALCATEGORYID"].ToString()));
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {
            txtSubHazard.Text = dt.Rows[0]["FLDHAZARD"].ToString();
            txtHazard.Text = dt.Rows[0]["FLDHAZARDNAME"].ToString();
            BindCheckBoxList(cblRecomendedPPE, dt.Rows[0]["FLDRECOMMENDEDPPE"].ToString());
            ViewState["OPERATIONALHAZARDID"] = dt.Rows[0]["FLDOPERATIONALHAZARDID"].ToString();
            txtoperational.Text = dt.Rows[0]["FLDOPERATIONAL"].ToString();
            txtAspect.Text = dt.Rows[0]["FLDASPECT"].ToString();
            txtelement.Text = dt.Rows[0]["FLDELEMENT"].ToString();
            if (dt.Rows[0]["FLDHAZARDTYPEID"].ToString().Equals("2"))
            {
                lblPPE.Visible = false;
                cblRecomendedPPE.Visible = false;
            }
        }
    }

    private void BindCheckBoxList(RadCheckBoxList cbl, string list)
    {
        foreach (string item in list.Split(','))
        {
            if (item.Trim() != "")
            {
                cbl.SelectedValue = item;
            }
        }
    }

    private string ReadCheckBoxList(RadCheckBoxList cbl)
    {
        string list = string.Empty;

        foreach (ButtonListItem item in cbl.Items)
        {
            if (item.Selected == true)
            {
                list = list + item.Value.ToString() + ",";
            }
        }
        list = list.TrimEnd(',');
        return list;
    }
}
