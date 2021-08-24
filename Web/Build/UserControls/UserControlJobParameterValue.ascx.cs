using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Telerik.Web.UI;

public partial class UserControlJobParameterValue : System.Web.UI.UserControl
{
    private Guid? _workorderid;
    protected void Page_Load(object sender, EventArgs e)
    {

        lstJobParameterValue.PreRender += new EventHandler(lstJobParameterValue_PreRender);
    }
    void lstJobParameterValue_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            foreach (RadListViewDataItem item in lstJobParameterValue.Items)
            {
                item.Edit = true;
            }
            lstJobParameterValue.Rebind();
        }
    }

    public Guid? WorkOrderId
    {
        get
        {
            return _workorderid;
        }
        set
        {
            _workorderid = value;
            BindData();
        }
    }
    public void Save(Guid workorderId)
    {
        StringBuilder sb = new StringBuilder();
        using (XmlWriter writer = XmlWriter.Create(sb))
        {
            writer.WriteStartElement("items");
            foreach (RadListViewEditableItem item in lstJobParameterValue.EditItems)
            {
                Hashtable newValues = new Hashtable();
                item.ExtractValues(newValues);
                RadLabel lblParameterType = (RadLabel)item.FindControl("lblParameterType");
                UserControlNumber txtJobParameternumber = (UserControlNumber)item.FindControl("txtJobParameternumber");
                UserControlDecimal txtJobParameterdecimal = (UserControlDecimal)item.FindControl("txtJobParameterdecimal");
                RadTextBox txtJobParameterValue = (RadTextBox)item.FindControl("txtJobParameterValue");
                RadDropDownList ddlparameteroption = item.FindControl("ddlparameteroption") as RadDropDownList;
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
    }
    public string IsValidParameter
    {
        get
        {
            StringBuilder sb = new StringBuilder();
            foreach (RadListViewEditableItem item in lstJobParameterValue.EditItems)
            {
                Hashtable newValues = new Hashtable();
                item.ExtractValues(newValues);
                RadLabel lblParameterType = (RadLabel)item.FindControl("lblParameterType");
                UserControlNumber txtJobParameternumber = (UserControlNumber)item.FindControl("txtJobParameternumber");
                UserControlDecimal txtJobParameterdecimal = (UserControlDecimal)item.FindControl("txtJobParameterdecimal");
                RadTextBox txtJobParameterValue = (RadTextBox)item.FindControl("txtJobParameterValue");
                RadDropDownList ddlparameteroption = item.FindControl("ddlparameteroption") as RadDropDownList;
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
    }
    private void BindData()
    {
        DataTable dt = PhoenixPlannedMaintenanceJobParameter.ListJobParameterValue(PhoenixSecurityContext.CurrentSecurityContext.VesselID, WorkOrderId);
        lstJobParameterValue.DataSource = dt;
        lstJobParameterValue.DataBind();

    }
    protected void lstJobParameterValue_ItemDataBound(object sender, RadListViewItemEventArgs e)
    {
        if (e.Item is RadListViewEditableItem || e.Item is RadListViewItem)
        {
            DataRowView dr = (DataRowView)DataBinder.GetDataItem(e.Item);

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
                        ddlparameteroption.SelectedValue = dr["FLDPARAMETEROPTIONID"].ToString();
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
                    txtJobParameternumber.Text = dr["FLDVALUE"].ToString();
                }
                if (lblParameterType.Text == "2")
                {
                    txtJobParameterdecimal.Visible = true;
                    txtJobParameternumber.Visible = false;
                    txtJobParameterValue.Visible = false;
                    ddlparameteroption.Visible = false;
                    txtJobParameterdecimal.Text = dr["FLDVALUE"].ToString();
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
}