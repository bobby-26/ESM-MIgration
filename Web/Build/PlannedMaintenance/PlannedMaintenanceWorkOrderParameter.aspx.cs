using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Xml;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;

public partial class PlannedMaintenance_PlannedMaintenanceWorkOrderParameter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuParameter.AccessRights = this.ViewState;
        MenuParameter.MenuList = toolbargrid.Show();
       
        if (!IsPostBack)
        {
            ViewState["woid"] = Request.QueryString["workorderid"] ?? string.Empty;            
        }
    }    
    protected void MenuParameter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                Guid workorderId = new Guid(ViewState["woid"].ToString());
                string p = IsValidParameter();
                if (!IsValidJobParameter(p))
                {
                    ucError.Visible = true;
                    return;
                }
                StringBuilder sb = new StringBuilder();
                using (XmlWriter writer = XmlWriter.Create(sb))
                {
                    writer.WriteStartElement("items");
                    foreach (GridDataItem item in gvJobParameter.Items)
                    {

                        RadLabel lblParameterType = (RadLabel)item.FindControl("lblParameterType");
                        RadLabel lblParameterName = (RadLabel)item.FindControl("lblParameterName");
                        RadLabel lblParameterId = (RadLabel)item.FindControl("lblParameterId");
                        RadLabel lblValueId = (RadLabel)item.FindControl("lblValueId");
                        UserControlNumber txtJobParameternumber = (UserControlNumber)item.FindControl("txtJobParameternumber");
                        UserControlDecimal txtJobParameterdecimal = (UserControlDecimal)item.FindControl("txtJobParameterdecimal");
                        RadTextBox txtJobParameterValue = (RadTextBox)item.FindControl("txtJobParameterValue");
                        RadDropDownList ddlparameteroption = item.FindControl("ddlparameteroption") as RadDropDownList;
                        Hashtable newValues = new Hashtable
                        {
                            { "FLDPARAMETERID", lblParameterId.Text },
                            { "FLDVALUEID", lblValueId.Text  },
                            { "FLDVALUE", "" },
                            { "FLDPARAMETEROPTIONID", "" },
                            { "FLDPARAMETERNAME", lblParameterName.Text }
                        };
                        if (lblParameterType.Text == "1" || lblParameterType.Text == "2")
                            newValues["FLDVALUE"] = txtJobParameternumber.Text;
                        if (lblParameterType.Text == "2")
                            newValues["FLDVALUE"] = txtJobParameterdecimal.Text;
                        else if (lblParameterType.Text == "4")
                        {
                            if (ddlparameteroption != null)
                            {
                                newValues["FLDVALUE"] = ddlparameteroption.SelectedValue == "" ? "" : ddlparameteroption.SelectedText;
                                newValues["FLDPARAMETEROPTIONID"] = ddlparameteroption.SelectedValue;
                            }
                        }
                        else if (lblParameterType.Text == "3")
                            newValues["FLDVALUE"] = txtJobParameterValue.Text;
                        writer.WriteStartElement("item");
                        foreach (DictionaryEntry itm in newValues)
                        {
                            writer.WriteStartElement(itm.Key.ToString());
                            writer.WriteValue(itm.Value ?? string.Empty);
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.Flush();
                }
                if (workorderId != null)
                    PhoenixPlannedMaintenanceJobParameter.InsertJobParameterValue(workorderId, PhoenixSecurityContext.CurrentSecurityContext.VesselID, sb.ToString());
                string script = "refreshParent();";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidJobParameter(string param)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        
        if (param.Trim().Length > 0)
            ucError.ErrorMessage = param;

        return (!ucError.IsError);
    }
    public string IsValidParameter()
    {
        StringBuilder sb = new StringBuilder();
        foreach (GridDataItem item in gvJobParameter.Items)
        {
           
            RadLabel lblParameterType = (RadLabel)item.FindControl("lblParameterType");
            RadLabel lblParameterName = (RadLabel)item.FindControl("lblParameterName");
            RadLabel lblParameterId = (RadLabel)item.FindControl("lblParameterId");
            UserControlNumber txtJobParameternumber = (UserControlNumber)item.FindControl("txtJobParameternumber");
            UserControlDecimal txtJobParameterdecimal = (UserControlDecimal)item.FindControl("txtJobParameterdecimal");
            RadTextBox txtJobParameterValue = (RadTextBox)item.FindControl("txtJobParameterValue");
            RadDropDownList ddlparameteroption = item.FindControl("ddlparameteroption") as RadDropDownList;

            Hashtable newValues = new Hashtable
            {
                { "FLDPARAMETERID", lblParameterId.Text },
                { "FLDVALUE", "" },
                { "FLDPARAMETEROPTIONID", "" },
                { "FLDPARAMETERNAME", lblParameterName.Text }
            };

            if (lblParameterType.Text == "1" || lblParameterType.Text == "2")
                newValues["FLDVALUE"] = txtJobParameternumber.Text;
            if (lblParameterType.Text == "2")
                newValues["FLDVALUE"] = txtJobParameterdecimal.Text;
            else if (lblParameterType.Text == "4")
            {
                if (ddlparameteroption != null)
                {
                    newValues["FLDVALUE"] = ddlparameteroption.SelectedValue == "" ? "" : ddlparameteroption.SelectedText;
                    newValues["FLDPARAMETEROPTIONID"] = ddlparameteroption.SelectedValue;
                }
            }
            else if (lblParameterType.Text == "3")
                newValues["FLDVALUE"] = txtJobParameterValue.Text;
            var itm = newValues["FLDVALUE"];
            if (itm == null || itm.ToString() == string.Empty)
                sb.Append(newValues["FLDPARAMETERNAME"] + " is Required.</br>");
        }
        return sb.ToString();
    }

    protected void gvJobParameter_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixPlannedMaintenanceJobParameter.ListJobParameterValue(PhoenixSecurityContext.CurrentSecurityContext.VesselID, new Guid(ViewState["woid"].ToString()));
        gvJobParameter.DataSource = dt;
    }

    protected void gvJobParameter_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.Item is GridDataItem)
            {
                e.Item.Selected = true;
                RadLabel lblParameterType = (RadLabel)e.Item.FindControl("lblParameterType");
                RadLabel lblParameterId = (RadLabel)e.Item.FindControl("lblParameterId");
                RadLabel lblParameterOptionId = (RadLabel)e.Item.FindControl("lblParameterOptionId");
                RadTextBox txtJobParameterValue = (RadTextBox)e.Item.FindControl("txtJobParameterValue");
                RadDropDownList ddlparameteroption = e.Item.FindControl("ddlparameteroption") as RadDropDownList;
                UserControlNumber txtJobParameternumber = (UserControlNumber)e.Item.FindControl("txtJobParameternumber");
                UserControlDecimal txtJobParameterdecimal = (UserControlDecimal)e.Item.FindControl("txtJobParameterdecimal");
                if (lblParameterType != null)
                {
                    if (lblParameterType.Text == "4")
                    {
                        if (lblParameterId != null)
                        {
                            ddlparameteroption.DataSource = PhoenixPlannedMaintenanceJobParameterOptions.JobParameterOptionList(new Guid(lblParameterId.Text));
                            ddlparameteroption.DataBind();
                            txtJobParameternumber.Visible = false;
                            ddlparameteroption.Items.Insert(0, new DropDownListItem("--Select--", ""));
                            //   ddlparameteroption.SelectedValue = lblParameterOptionId.Text;
                            ddlparameteroption.SelectedValue = drv["FLDPARAMETEROPTIONID"].ToString();
                        }
                        txtJobParameterValue.Visible = false;
                        ddlparameteroption.Visible = true;
                        txtJobParameterdecimal.Visible = false;
                    }
                    else
                    {
                        txtJobParameterValue.Visible = true;
                        ddlparameteroption.Visible = false;
                    }
                    if (lblParameterType.Text == "1")
                    {
                        txtJobParameternumber.Visible = true;
                        txtJobParameterValue.Visible = false;
                        ddlparameteroption.Visible = false;
                        txtJobParameterdecimal.Visible = false;
                        txtJobParameternumber.Text = drv["FLDVALUE"].ToString();
                    }
                    if (lblParameterType.Text == "2")
                    {
                        txtJobParameterdecimal.Visible = true;
                        txtJobParameternumber.Visible = false;
                        txtJobParameterValue.Visible = false;
                        ddlparameteroption.Visible = false;
                        txtJobParameterdecimal.Text = drv["FLDVALUE"].ToString();
                    }
                    if (lblParameterType.Text == "3")
                    {
                        txtJobParameterdecimal.Visible = false;
                        txtJobParameternumber.Visible = false;
                        txtJobParameterValue.Visible = true;
                        ddlparameteroption.Visible = false;

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
}