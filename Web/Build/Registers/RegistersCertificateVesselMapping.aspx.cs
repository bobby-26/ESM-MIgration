using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersCertificateVesselMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuVesselMapping.AccessRights = this.ViewState;
            MenuVesselMapping.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["DTKEY"] = "";
                ViewState["CERTIFICATEID"] = "";

                if (Request.QueryString["DTKEY"] != null)
                {
                    ViewState["DTKEY"] = Request.QueryString["DTKEY"].ToString();
                }
                if (Request.QueryString["CERTIFICATEID"] != null)
                {
                    ViewState["CERTIFICATEID"] = Request.QueryString["CERTIFICATEID"].ToString();
                }

                BindMapping();
            }
        }
        catch (Exception ex)
        {

            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuVesselMapping_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string mapping = ReadCheckBoxList(cblVessel);
                PhoenixRegistersCertificates.MapVesselList(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["DTKEY"].ToString()), mapping);

				PhoenixRegistersCertificates.ExcludevesselInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
					int.Parse(ViewState["CERTIFICATEID"].ToString()), mapping);


				ucStatus.Text = "Selected Vessels Removed Successfully";
                BindMapping();

                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1',null,true);", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindCheckBoxList(RadCheckBoxList cbl, string list)
    {
        string[] vessel = list.Split(',');
        foreach (string item in vessel)
        {
            foreach (ButtonListItem li in cbl.Items)
            {
                if (li.Value == item)
                {
                    li.Selected = true;
                }
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

    private void BindVesselTypes()
    {
        cblVessel.Items.Clear();
        DataSet ds = PhoenixRegistersCertificates.SelectedTypeWiseVesselList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , int.Parse(ViewState["CERTIFICATEID"].ToString()));
        cblVessel.DataSource = ds;
        cblVessel.DataBindings.DataTextField = "FLDVESSELNAME";
        cblVessel.DataBindings.DataValueField = "FLDVESSELID";
        cblVessel.DataBind();
    }

    private void BindMapping()
    {
        BindVesselTypes();
        DataSet ds = PhoenixRegistersCertificates.EditCertificates(int.Parse(ViewState["CERTIFICATEID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            string mapping = ds.Tables[0].Rows[0]["FLDEXCLUDEVESSELLIST"].ToString();
            txtCertificate.Text = ds.Tables[0].Rows[0]["FLDCERTIFICATENAME"].ToString();
            BindCheckBoxList(cblVessel, mapping);
        }
    }

    protected void SelectAll(object sender, EventArgs e)
    {
        if (chkCheckAll.Checked == true)
        {
            foreach (ButtonListItem item in cblVessel.Items)
            {
                item.Selected = true;
            }
        }
        else
        {
            foreach (ButtonListItem item in cblVessel.Items)
            {
                item.Selected = false;
            }
        }
    }
}
