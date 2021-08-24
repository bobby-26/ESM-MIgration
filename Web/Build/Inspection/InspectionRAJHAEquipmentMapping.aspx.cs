using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Integration;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionRAJHAEquipmentMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {

                ViewState["Categoryid"] = "";
                if ((Request.QueryString["Categoryid"] != null) && (Request.QueryString["Categoryid"] != ""))
                {
                    ViewState["Categoryid"] = Request.QueryString["Categoryid"].ToString();
                }

                ViewState["typeid"] = "";
                if ((Request.QueryString["typeid"] != null) && (Request.QueryString["typeid"] != ""))
                {
                    ViewState["typeid"] = Request.QueryString["typeid"].ToString();
                }

                btnShowComponents.Attributes.Add("onclick", "return showPickList('spnPickListComponent', 'codehelp1', '', '../Common/CommonPickListGlobalComponent.aspx'); ");
            }
            BindComponents();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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

    protected void lnkComponentAdd_Click(object sender, EventArgs e)
    {
        if (General.GetNullableGuid(txtComponentId.Text) != null)
        {

            PhoenixInspectionOperationalRiskControls.UpdateUndesirableEventComponents(new Guid(ViewState["Categoryid"].ToString()), new Guid(txtComponentId.Text));
            ucStatus.Text = "Component added.";
            txtComponentId.Text = "";
            txtComponentCode.Text = "";
            txtComponentName.Text = "";
            BindComponents();
        }
    }

    protected void BindComponents()
    {
        DataSet dss = PhoenixInspectionOperationalRiskControls.UndesirableEventComponentList(ViewState["Categoryid"] == null ? null : General.GetNullableGuid(ViewState["Categoryid"].ToString()));
        int number = 1;
        if (dss.Tables[0].Rows.Count > 0)
        {
            tblcomponents.Rows.Clear();
            foreach (DataRow dr in dss.Tables[0].Rows)
            {
                RadCheckBox cb = new RadCheckBox();
                cb.ID = dr["FLDCOMPONENTID"].ToString();
                cb.Text = "";
                cb.Checked = true;
                cb.AutoPostBack = true;
                cb.CheckedChanged += new EventHandler(com_CheckedChanged);
                HyperLink hl = new HyperLink();
                hl.Text = dr["FLDCOMPONENTNAME"].ToString();
                hl.ID = "hlink2" + number.ToString();
                hl.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");
                HtmlTableRow tr = new HtmlTableRow();
                HtmlTableCell tc = new HtmlTableCell();
                tc.Controls.Add(cb);
                tr.Cells.Add(tc);
                tc = new HtmlTableCell();
                tc.Controls.Add(hl);
                tr.Cells.Add(tc);
                tblcomponents.Rows.Add(tr);
                tc = new HtmlTableCell();
                tc.InnerHtml = "<br/>";
                tr.Cells.Add(tc);
                tblcomponents.Rows.Add(tr);
                number = number + 1;
            }
            divComponents.Visible = true;
        }
        else
            divComponents.Visible = false;
    }

    void com_CheckedChanged(object sender, EventArgs e)
    {
        RadCheckBox c = (RadCheckBox)sender;
        if (c.Checked == false)
        {
            PhoenixInspectionOperationalRiskControls.UndesirableEventComponentDelete(new Guid(ViewState["Categoryid"].ToString()), new Guid(c.ID));

            string txt = "";

            ucStatus.Text = txt + "deleted.";
            BindComponents();
        }
    }
}
