using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.StandardForm;
using System.Web;

public partial class StandardFormBearingMeasuring : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {           
            if (Request.QueryString["mode"] != null)
            {                
                cmdPrint.Visible = true;
            }
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                cmdHiddenConfirm.Attributes.Add("style", "display:none");
                ViewState["DONEID"] = Request.QueryString["DONEID"];
                ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"];
                BindData(pnlStandardForm);
                if (Request.QueryString["mode"] == null && (ViewState["DONEID"] == null || ViewState["DONEID"].ToString() == string.Empty))
                {
                    PrePopulateData(new Guid(ViewState["WORKORDERID"].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void PrePopulateData(Guid gWorkOrderId)
    {
        ResetControls(pnlStandardForm);
        LoadVesselData(gWorkOrderId);
        ViewState["DONEID"] = null;
    }
    private void LoadVesselData(Guid WorkOrderId)
    {
        DataTable dt = PhoenixStandardForm.ListVesselData(WorkOrderId);
        if (dt.Rows.Count > 0)
        {
            txtvesselName.Text = dt.Rows[0]["FLDVESSELNAME"].ToString();           
            txtEngineTye.Text = dt.Rows[0]["FLDENGINETYPE"].ToString();         
            
            if (dt.Rows[0]["FLDMAINENGINE"].ToString() != DBNull.Value.ToString() && dt.Rows[0]["FLDAUXENGINE"].ToString() != DBNull.Value.ToString())
                txtMainEngineAuxEngine.Text = dt.Rows[0]["FLDMAINENGINE"].ToString() + "/" + dt.Rows[0]["FLDAUXENGINE"].ToString();
            else
                txtMainEngineAuxEngine.Text = dt.Rows[0]["FLDMAINENGINE"].ToString() + dt.Rows[0]["FLDAUXENGINE"].ToString();

        }
    }
    #region StandardForm Data Save Methods
    protected void StandardForm_TabStripCommand(object sender, EventArgs e)
    {
        DateTime resultDate;
        if (DateTime.TryParse(txtDate.Text, out resultDate)
        && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Date should be earlier than Current Date.";
            ucError.Visible = true;
        }

        else
        {
            if (string.IsNullOrEmpty(txtDate.Text))
            {
                ucError.ErrorMessage = "Date is required.";
                ucError.Visible = true;
            }
            else
            {
                try
                {
                    if (ViewState["DONEID"] == null || ViewState["DONEID"].ToString().Trim() == string.Empty)
                    {
                        string data = string.Empty;
                        FetchControlValue(pnlStandardForm, ref data);
                        DataTable dt = PhoenixStandardForm.InsertStandardFormData(Request.Url.Segments[Request.Url.Segments.Length - 1], new Guid(ViewState["WORKORDERID"].ToString()), data);
                        if (dt.Rows.Count > 0)
                            ViewState["DONEID"] = dt.Rows[0][0].ToString();
                        BindData(pnlStandardForm);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", "javascript:parent.parent.fnReloadList(null,'ifMoreInfo',null);", true);
                    }
                    else if (ViewState["DONEID"] != null || ViewState["DONEID"].ToString().Trim() != string.Empty)
                    {
                        string data = string.Empty;
                        FetchControlValue(pnlStandardForm, ref data);
                        PhoenixStandardForm.UpdateStandardFormData(new Guid(ViewState["DONEID"].ToString()), Server.HtmlEncode(data));
                        BindData(pnlStandardForm);
                    }

                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
            }
        }
    }
    protected void StandardFormConfirm_TabStripCommand(object sender, EventArgs e)
    {
        if (ViewState["DONEID"] != null)
        {
            ViewState["DONEID"] = null;
            StandardForm_TabStripCommand(this, new EventArgs());
        }
        else
        {
            ucError.ErrorMessage = "Please Save the information before confirm";
            ucError.Visible = true;
        }
    }
    private void BindData(Control PanelControl)
    {
        if (ViewState["DONEID"] != null && ViewState["DONEID"].ToString() != string.Empty)
        {
            DataTable dt = PhoenixStandardForm.ListStandardFormData(new Guid(ViewState["DONEID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                Control c = null;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    c = PanelControl.FindControl(dt.Rows[i]["FLDCONTROLID"].ToString());
                    if (c != null)
                    {
                        SetControlValue(c,HttpUtility.HtmlDecode( dt.Rows[i]["FLDCONTROLDATA"].ToString()));
                    }
                }
            }
        }
    }
    private void FetchControlValue(Control parent, ref string value)
    {
        string strIgnoreIteration = ",ASP.usercontrols_usercontroldate_ascx,ASP.usercontrols_usercontrolmasknumber_ascx,System.Web.UI.WebControls.CheckBoxList,";
        foreach (Control c in parent.Controls)
        {
            if (c.Controls.Count > 0 && !strIgnoreIteration.Contains("," + c.GetType().ToString() + ","))
            {
                FetchControlValue(c, ref value);
            }
            else
            {
                switch (c.GetType().ToString())
                {
                    case "System.Web.UI.WebControls.TextBox":
                        value += ((TextBox)c).ID + "~" + ((TextBox)c).Text.Replace(",", "┐").Replace("'", "¿") + ",";
                        break;
                    case "System.Web.UI.WebControls.CheckBox":
                        value += ((CheckBox)c).ID + "~" + (((CheckBox)c).Checked ? "1" : "0") + ",";
                        break;
                    case "System.Web.UI.WebControls.CheckBoxList":
                        {
                            string tempstr = ((CheckBoxList)c).ID + "~";
                            foreach (ListItem li in ((CheckBoxList)c).Items)
                                tempstr += li.Selected ? li.Value + "|" : string.Empty;
                            value += tempstr.TrimEnd('|');
                            break;
                        }
                    case "System.Web.UI.WebControls.RadioButton":
                        value += ((RadioButton)c).ID + "~" + (((RadioButton)c).Checked ? "1" : "0") + ",";
                        break;
                    case "System.Web.UI.WebControls.RadioButtonList":
                        value += ((RadioButtonList)c).ID + "~" + ((RadioButtonList)c).SelectedValue + ",";
                        break;
                    case "System.Web.UI.WebControls.DropDownList":
                        value += ((DropDownList)c).ID + "~" + ((DropDownList)c).SelectedValue + ",";
                        break;
                    case "ASP.usercontrols_usercontroldate_ascx":
                        value += ((UserControlDate)c).ID + "~" + (General.GetNullableDateTime(((UserControlDate)c).Text).HasValue ? ((UserControlDate)c).Text : string.Empty) + ",";
                        break;
                    case "ASP.usercontrols_usercontrolmasknumber_ascx":
                        value += ((UserControlMaskNumber)c).ID + "~" + (General.GetNullableDecimal(((UserControlMaskNumber)c).Text).HasValue ? ((UserControlMaskNumber)c).Text : string.Empty) + ",";
                        break;
                    default:
                        break;
                }
            }
        }
    }
    private void SetControlValue(Control c, string value)
    {
        switch (c.GetType().ToString())
        {
            case "System.Web.UI.WebControls.TextBox":
                ((TextBox)c).Text = value.Replace("┐", ",").Replace("¿", "'");
                break;
            case "System.Web.UI.WebControls.CheckBox":
                ((CheckBox)c).Checked = (value == "1" ? true : false);
                break;
            case "System.Web.UI.WebControls.RadioButton":
                ((RadioButton)c).Checked = (value == "1" ? true : false);
                break;
            case "System.Web.UI.WebControls.CheckBoxList":
                {
                    string[] tempstr = value.Split('|');
                    foreach (string str in tempstr)
                    {
                        if (string.IsNullOrEmpty(str)) continue;
                        ((CheckBoxList)c).Items.FindByValue(str).Selected = true;
                    }
                    break;
                }
            case "System.Web.UI.WebControls.RadioButtonList":
                if (value.Trim().ToString() != string.Empty) ((RadioButtonList)c).SelectedValue = value;
                break;
            case "System.Web.UI.WebControls.DropDownList":
                ((DropDownList)c).SelectedValue = value;
                break;
            case "ASP.usercontrols_usercontroldate_ascx":
                ((UserControlDate)c).Text = value;
                break;
            case "ASP.usercontrols_usercontrolmasknumber_ascx":
                ((UserControlMaskNumber)c).Text = value;
                break;
            default:
                break;
        }
    }
    private void ResetControls(Control parent)
    {
        try
        {
            string strIgnoreIteration = ",ASP.usercontrols_usercontroldate_ascx,ASP.usercontrols_usercontrolmasknumber_ascx,System.Web.UI.WebControls.CheckBoxList,";
            foreach (Control c in parent.Controls)
            {
                if (c.Controls.Count > 0 && !strIgnoreIteration.Contains("," + c.GetType().ToString() + ","))
                {
                    ResetControls(c);
                }
                else
                {
                    switch (c.GetType().ToString())
                    {
                        case "System.Web.UI.WebControls.TextBox":
                            ((TextBox)c).Text = string.Empty;
                            break;
                        case "System.Web.UI.WebControls.CheckBox":
                            ((CheckBox)c).Checked = false;
                            break;
                        case "System.Web.UI.WebControls.CheckBoxList":
                            {
                                foreach (ListItem li in ((CheckBoxList)c).Items)
                                    li.Selected = false;
                                break;
                            }
                        case "System.Web.UI.WebControls.RadioButton":
                            ((RadioButton)c).Checked = false;
                            break;
                        case "System.Web.UI.WebControls.RadioButtonList":
                            ((RadioButtonList)c).SelectedIndex = -1;
                            break;
                        case "System.Web.UI.WebControls.DropDownList":
                            ((DropDownList)c).SelectedIndex = -1;
                            break;
                        case "ASP.usercontrols_usercontroldate_ascx":
                            ((UserControlDate)c).Text = string.Empty;
                            break;
                        case "ASP.usercontrols_usercontrolmasknumber_ascx":
                            ((UserControlMaskNumber)c).Text = string.Empty;
                            break;
                        default:
                            break;
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
    #endregion
}
