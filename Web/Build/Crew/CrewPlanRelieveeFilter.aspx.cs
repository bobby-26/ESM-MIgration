using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;
using System.Data;
public partial class CrewPlanRelieveeFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();        
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        PlanRelieveeFilterMain.AccessRights = this.ViewState;
        PlanRelieveeFilterMain.MenuList = toolbar.Show();

        if (!IsPostBack)
        {            
            txtReliefDue.Text = "90";
            ucVessel.VesselList = PhoenixRegistersVessel.ListVessel(null, "",0);      
            txtName.Focus();
            BindGroupRankList();

        }
    }

    private void BindGroupRankList()
    {
        ddlGroupRank.SelectedValue = string.Empty;
        ddlGroupRank.Text = "";

        DataSet ds = PhoenixRegistersGroupRank.ListGroupRank();
        ddlGroupRank.DataSource = ds;
        ddlGroupRank.DataBind();
    }

    protected void PlanRelieveeFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        if (CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();
            if (!IsValidDateTime())
            {
                ucError.Visible = true;
                return;
            }

            string grouprankList = GetCsvValue(ddlGroupRank);

            criteria.Clear();
            criteria.Add("txtReliefDue", txtReliefDue.Text);
            criteria.Add("txtName", txtName.Text);
            criteria.Add("ucRank", ucRank.selectedlist);
            criteria.Add("ucVessel", ucVessel.SelectedVessel);
            criteria.Add("ucVesselType", lstVesselType.SelectedVesseltype);
            criteria.Add("ucPool", ucPool.SelectedPool);//ucPool.SelectedPool);
            criteria.Add("ucZone", ucZone.selectedlist);
            criteria.Add("lstNationality", string.Empty);//lstNationality.SelectedList);
            criteria.Add("ucPrincipal", ucPrincipal.SelectedAddress);
            criteria.Add("txtFromDate", txtFromDate.Text);
            criteria.Add("txtToDate", txtFromTo.Text);
            criteria.Add("ucRankGroup", grouprankList);
            criteria.Add("chkNotPlanned", chkNotPlanned.Checked.Value ? "1" : string.Empty);
            Filter.CurrentPlanRelieveeFilterSelection = criteria;
        }

        if (Request.QueryString["Ispopup"] != null && Request.QueryString["Ispopup"].ToString() == "1")           // called from popup
        {

            string refreshname = "reliefplan";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                        "BookMarkScript", "top.closeTelerikWindow('Filter'," + (refreshname == string.Empty ? "null" : "'" + refreshname + "'") + ");", true);
        }
        else
        {
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
        }



    }

    private string GetCsvValue(RadComboBox radComboBox)
    {
        var list = radComboBox.CheckedItems;
        string csv = string.Empty;
        if (list.Count != 0)
        {
            csv = ",";
            foreach (var item in list)
            {
                csv = csv + item.Value + ",";
            }
        }
        return csv;
    }
    protected void CalulateDays(object sender, EventArgs e)
	{
		try
		{
			
			if (txtFromTo.Text != null && txtFromDate.Text!=null)
			{
				DateTime dtfrom = Convert.ToDateTime(txtFromDate.Text);
				DateTime dtto = Convert.ToDateTime(txtFromTo.Text);
				
				TimeSpan ts = dtto - dtfrom;
				string days = ts.Days.ToString();
				txtReliefDue.Text = days;

			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}

	protected void CalculateDatetime(object sender, EventArgs e)
	{
		try
		{
			
			if (txtFromDate.Text != null && txtFromTo.Text != null)
			{
				DateTime dt = DateTime.Parse(txtFromDate.Text).AddDays(int.Parse(txtReliefDue.Text));
				txtFromTo.Text = dt.ToString();
			}
			
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}

	public bool IsValidDateTime()
	{
		ucError.HeaderMessage = "Please provide the following required information";
		if (txtFromDate.Text != "" && txtFromTo.Text != null)
		{
			DateTime dtfrom = Convert.ToDateTime(txtFromDate.Text);
			DateTime dtto = Convert.ToDateTime(txtFromTo.Text);
			if (dtto < dtfrom)
				ucError.ErrorMessage = "To date should be greater than From date.";
		}
		return (!ucError.IsError);
	}
	protected void ucPrincipal_TextChangedEvent(object sender, EventArgs e)
	{
		StringBuilder strVesselType = new StringBuilder();
		ucPrincipal.SelectedAddress = General.GetNullableInteger(ucPrincipal.SelectedAddress) == null ? "" : ucPrincipal.SelectedAddress;
        ucVessel.VesselList = PhoenixRegistersVessel.ListVessel(null, General.GetNullableString(ucPrincipal.SelectedAddress), 0, lstVesselType.SelectedVesseltype);        
    }
}
