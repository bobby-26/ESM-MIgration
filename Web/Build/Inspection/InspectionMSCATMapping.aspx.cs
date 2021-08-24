using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionMSCATMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuMapping.MenuList = toolbar.Show();
            if (!IsPostBack)
            {

                //MenuMapping.SetTrigger(pnlContactType);

                if (Request.QueryString["type"].ToString() == "1") //Mapping accident description for contact type
                {
                    // ucTitle.Text = "Accident Description Mapping";
                    lblSource.Text = "Contact Type";
                    lblDestination.Text = "Accident Description";
                }
                if (Request.QueryString["type"].ToString() == "2") //Mapping contact type for immediate causes
                {
                    // ucTitle.Text = "Contact Type Mapping";
                    lblSource.Text = "Immediate Cause";
                    lblDestination.Text = "Contact Type";
                }
                if (Request.QueryString["type"].ToString() == "3") //Mapping immediate causes for basic cause
                {
                    // ucTitle.Text = "Immediate Cause Mapping";
                    lblSource.Text = "Basic Cause";
                    lblDestination.Text = "Immediate Cause";
                }
                if (Request.QueryString["type"].ToString() == "4") //Mapping basic causes for control actions
                {
                    Title = "Basic Cause Mapping";
                    // ucTitle.Text = "Basic Cause Mapping";
                    lblSource.Text = "Control Action Needs";
                    lblDestination.Text = "Basic Cause";
                }
                BindMappingDropdown();
                BindSourceCheckbox();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void BindSourceCheckbox()
    {
        if (Request.QueryString["type"].ToString() == "1")
        {
            cblSource.DataSource = PhoenixInspectionContractType.ListContactType();
            cblSource.DataTextField = "FLDCONTACTTYPENAME";
            cblSource.DataValueField = "FLDCONTACTTYPEID";
            cblSource.DataBind();
        }
        if (Request.QueryString["type"].ToString() == "2")
        {
            cblSource.DataSource = PhoenixInspectionImmediateCause.ListImmediateCause(null);
            cblSource.DataTextField = "FLDIMMEDIATECAUSENAME";
            cblSource.DataValueField = "FLDIMMEDIATECAUSEID";
            cblSource.DataBind();
        }
        if (Request.QueryString["type"].ToString() == "3")
        {
            cblSource.DataSource = PhoenixInspectionBasicCause.ListBasicCause(null);
            cblSource.DataTextField = "FLDBASICCAUSENAME";
            cblSource.DataValueField = "FLDBASICCAUSEID";
            cblSource.DataBind();
        }
        if (Request.QueryString["type"].ToString() == "4")
        {
            cblSource.DataSource = PhoenixInspectionMscatControlActionNeeded.ListControlActionNeeded(null);
            cblSource.DataTextField = "FLDCONTROLACTIONNEEDEDNAME";
            cblSource.DataValueField = "FLDCONTROLACTIONNEEDEDID";
            cblSource.DataBind();
        }
    }

    protected void BindMappingDropdown()
    {
        if (Request.QueryString["type"].ToString() == "1")
        {
            ddlMapping.DataSource = PhoenixRegistersHard.ListHard(1, 206);
            ddlMapping.DataTextField = "FLDHARDNAME";
            ddlMapping.DataValueField = "FLDHARDCODE";
            ddlMapping.DataBind();
            ddlMapping.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        }
        if (Request.QueryString["type"].ToString() == "2")
        {
            ddlMapping.DataSource = PhoenixInspectionContractType.ListContactType();
            ddlMapping.DataTextField = "FLDCONTACTTYPE";
            ddlMapping.DataValueField = "FLDCONTACTTYPEID";
            ddlMapping.DataBind();
            ddlMapping.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        }
        if (Request.QueryString["type"].ToString() == "3")
        {
            ddlMapping.DataSource = PhoenixInspectionImmediateCause.ListImmediateCause(null);
            ddlMapping.DataTextField = "FLDIMMEDIATECAUSE";
            ddlMapping.DataValueField = "FLDIMMEDIATECAUSEID";
            ddlMapping.DataBind();
            ddlMapping.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        }
        if (Request.QueryString["type"].ToString() == "4")
        {
            ddlMapping.DataSource = PhoenixInspectionBasicCause.ListBasicCause(null);
            ddlMapping.DataTextField = "FLDBASICCAUSE";
            ddlMapping.DataValueField = "FLDBASICCAUSEID";
            ddlMapping.DataBind();
            ddlMapping.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
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

    protected void MenuMapping_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (Request.QueryString["type"].ToString() == "1")
                {
                    string source = ReadCheckBoxList(cblSource);
                    if (!IsValidMapping())
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixInspectionContractType.MapAccidentDescription(source, int.Parse(ddlMapping.SelectedValue));
                    ucStatus.Text = "Contact types are mapped successfully.";
                }
                if (Request.QueryString["type"].ToString() == "2")
                {
                    string source = ReadCheckBoxList(cblSource);
                    if (!IsValidMapping())
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixInspectionImmediateCause.MapContactType(source, new Guid(ddlMapping.SelectedValue));
                    ucStatus.Text = "Immediate Causes are mapped successfully.";
                }
                if (Request.QueryString["type"].ToString() == "3")
                {
                    string source = ReadCheckBoxList(cblSource);
                    if (!IsValidMapping())
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixInspectionBasicCause.MapImmediateCause(source, new Guid(ddlMapping.SelectedValue));
                    ucStatus.Text = "Basic Causes are mapped successfully.";
                }
                if (Request.QueryString["type"].ToString() == "4")
                {
                    string source = ReadCheckBoxList(cblSource);
                    if (!IsValidMapping())
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixInspectionMscatControlActionNeeded.MapBasicCause(source, new Guid(ddlMapping.SelectedValue));
                    ucStatus.Text = "Control Action Needs are mapped successfully.";
                }
                BindCheckbox();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidMapping()
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (Request.QueryString["type"].ToString() == "1")
        {
            if (General.GetNullableInteger(ddlMapping.SelectedValue) == null)
                ucError.ErrorMessage = "Accident Description is required.";
        }
        if (Request.QueryString["type"].ToString() == "2")
        {
            if (General.GetNullableGuid(ddlMapping.SelectedValue) == null)
                ucError.ErrorMessage = "Contact Type is required.";
        }
        if (Request.QueryString["type"].ToString() == "3")
        {
            if (General.GetNullableGuid(ddlMapping.SelectedValue) == null)
                ucError.ErrorMessage = "Immediate Cause is required.";
        }
        if (Request.QueryString["type"].ToString() == "4")
        {
            if (General.GetNullableGuid(ddlMapping.SelectedValue) == null)
                ucError.ErrorMessage = "Basic Cause is required.";
        }

        return (!ucError.IsError);
    }

    protected void BindCheckbox()
    {
        if (Request.QueryString["type"].ToString() == "1")
        {
            DataSet ds = PhoenixInspectionContractType.ListContactType(General.GetNullableInteger(ddlMapping.SelectedValue));
            DataTable dt = ds.Tables[0];
            if (General.GetNullableInteger(ddlMapping.SelectedValue) != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = (DataRow)dt.Rows[i];
                    string s = dr["FLDCONTACTTYPEID"].ToString();
                    if (cblSource.Items.FindByValue(s) != null)
                        cblSource.Items.FindByValue(s).Selected = true;
                }
            }
        }
        if (Request.QueryString["type"].ToString() == "2")
        {
            DataSet ds = PhoenixInspectionImmediateCause.ListImmediateCause(General.GetNullableGuid(ddlMapping.SelectedValue));
            DataTable dt = ds.Tables[0];
            if (General.GetNullableGuid(ddlMapping.SelectedValue) != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = (DataRow)dt.Rows[i];
                    string s = dr["FLDIMMEDIATECAUSEID"].ToString();
                    if (cblSource.Items.FindByValue(s) != null)
                        cblSource.Items.FindByValue(s).Selected = true;
                }
            }
        }
        if (Request.QueryString["type"].ToString() == "3")
        {
            DataSet ds = PhoenixInspectionBasicCause.ListBasicCause(General.GetNullableGuid(ddlMapping.SelectedValue));
            DataTable dt = ds.Tables[0];
            if (General.GetNullableGuid(ddlMapping.SelectedValue) != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = (DataRow)dt.Rows[i];
                    string s = dr["FLDBASICCAUSEID"].ToString();
                    if (cblSource.Items.FindByValue(s) != null)
                        cblSource.Items.FindByValue(s).Selected = true;
                }
            }
        }
        if (Request.QueryString["type"].ToString() == "4")
        {
            DataSet ds = PhoenixInspectionMscatControlActionNeeded.ListControlActionNeeded(General.GetNullableGuid(ddlMapping.SelectedValue));
            DataTable dt = ds.Tables[0];
            if (General.GetNullableGuid(ddlMapping.SelectedValue) != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = (DataRow)dt.Rows[i];
                    string s = dr["FLDCONTROLACTIONNEEDEDID"].ToString();
                    if (cblSource.Items.FindByValue(s) != null)
                        cblSource.Items.FindByValue(s).Selected = true;
                }
            }
        }
    }

    protected void ddlMapping_Changed(object sender, EventArgs e)
    {
        cblSource.Items.Clear();
        BindSourceCheckbox();
        BindCheckbox();
    }
}
