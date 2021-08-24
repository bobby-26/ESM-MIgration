using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersCertificateVesselTypeMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuVesselTypeMapping.AccessRights = this.ViewState;
            MenuVesselTypeMapping.MenuList = toolbar.Show();
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

    protected void MenuVesselTypeMapping_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string mapping = ReadCheckBoxList(cblVesselType);
                PhoenixRegistersCertificates.MapVesselTypes(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["DTKEY"].ToString()), mapping);

                ucStatus.Text = "Vessel Types are mapped successfully";
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
        cblVesselType.Items.Clear();
        DataSet ds = PhoenixRegistersVesselType.ListVesselType(1);
        cblVesselType.DataSource = ds;
        cblVesselType.DataBindings.DataTextField = "FLDTYPEDESCRIPTION";
        cblVesselType.DataBindings.DataValueField = "FLDVESSELTYPEID";
        cblVesselType.DataBind();
    }

    private void BindMapping()
    {
        BindVesselTypes();
        DataSet ds = PhoenixRegistersCertificates.EditCertificates(int.Parse(ViewState["CERTIFICATEID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            string mapping = ds.Tables[0].Rows[0]["FLDVESSELTYPEAPPLICABLE"].ToString();
            txtCertificate.Text = ds.Tables[0].Rows[0]["FLDCERTIFICATENAME"].ToString();
            BindCheckBoxList(cblVesselType, mapping);
        }
    }

    protected void SelectAll(object sender, EventArgs e)
    {
        if (chkCheckAll.Checked == true)
        {
            foreach (ButtonListItem item in cblVesselType.Items)
            {
                item.Selected = true;
            }
        }
        else
        {
            foreach (ButtonListItem item in cblVesselType.Items)
            {
                item.Selected = false;
            }
        }
    }
}
