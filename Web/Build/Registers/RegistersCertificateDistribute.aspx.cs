using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersCertificateDistribute : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Distribute", "DISTRIBUTE",ToolBarDirection.Right);
            MenuDistribute.AccessRights = this.ViewState;
            MenuDistribute.MenuList = toolbar.Show();
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

                BindVessel();
            }
        }
        catch (Exception ex)
        {

            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuDistribute_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("DISTRIBUTE"))
			{
				string Vessellist = ReadCheckBoxList(cblVessel);
				PhoenixRegistersCertificates.Distribute(PhoenixSecurityContext.CurrentSecurityContext.UserCode,int.Parse(ViewState["CERTIFICATEID"].ToString())
					, Vessellist);

				ucStatus.Text = "Certificate Distributed.";
			}
		}
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
    private void BindVessel()
    {
        DataSet ds = PhoenixRegistersCertificates.ApplicableVessel(int.Parse(ViewState["CERTIFICATEID"].ToString()));

        cblVessel.DataSource = ds.Tables[1];
        cblVessel.DataBindings.DataTextField = "FLDVESSELNAME";
        cblVessel.DataBindings.DataValueField = "FLDVESSELID";
        cblVessel.DataBind();

        txtCertificate.Text = ds.Tables[0].Rows[0]["FLDCERTIFICATENAME"].ToString();
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
