using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class InspectionWorstCaseUndesirableMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {

                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Save", "SAVE");
                MenuMapping.MenuList = toolbar.Show();
                MenuMapping.SetTrigger(pnlTypeMapping);

                ViewState["OILMAJORCOMID"] = "";

                if (Request.QueryString["Hazardid"] != null)
                    ViewState["HAZARDID"] = Request.QueryString["Hazardid"].ToString();

                if (Request.QueryString["Hazard"] != null)
                    txtWorstCase.Text = Request.QueryString["Hazard"].ToString();

                BindCheckbox();
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void MenuMapping_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            String scriptpopupopen = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo', 'keeppopupopen');");

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                string source = ReadCheckBoxList(cblType);

                PhoenixInspectionOperationalRiskControls.UpdateWorstCaseUndesirableEvent(General.GetNullableInteger(ViewState["HAZARDID"].ToString())
                    , source);
                ucStatus.Text = "Types are mapped successfully.";

                BindData();

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupopen, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindCheckbox()
    {
        cblType.Items.Clear();
        cblType.DataSource = PhoenixInspectionOperationalRiskControls.ListUndesirableEvent();
        cblType.DataTextField = "FLDQUICKNAME";
        cblType.DataValueField = "FLDQUICKCODE";
        cblType.DataBind();
    }
    private void BindData()
    {
        BindCheckbox();
        DataTable dt = PhoenixInspectionOperationalRiskControls.ListWorstCaseUndesirableEvent(General.GetNullableInteger(ViewState["HAZARDID"].ToString()));

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            DataRow dr = (DataRow)dt.Rows[i];
            string s = dr["FLDEVENTID"].ToString();
            if (cblType.Items.FindByValue(s) != null)
                cblType.Items.FindByValue(s).Selected = true;
        }
    }
    private string ReadCheckBoxList(CheckBoxList cbl)
    {
        string list = string.Empty;

        foreach (ListItem item in cbl.Items)
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
