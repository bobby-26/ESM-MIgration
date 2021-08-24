using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Web.UI;
using SouthNests.Phoenix.PayRoll;

public partial class PayRoll_PayRollTaxRateIndia : PhoenixBasePage
{
    int usercode = 0;
    int vesselId = 0;
    Guid Id = Guid.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
		if (string.IsNullOrWhiteSpace(Request.QueryString["id"]) == false)
        {
            Id = new Guid(Request.QueryString["id"]);
        }
        ShowToolBar();

        if (IsPostBack == false)
        {
            LoadTaxableYearList();
            GetEditData();
        }
    }

    private void LoadTaxableYearList()
    {
        ddlPayroll.DataSource = PhoenixPayRollIndia.PayRollTaxIndiaList();
        ddlPayroll.DataTextField = "FLDREVISION";
        ddlPayroll.DataValueField = "FLDPAYROLLTAXID";
        ddlPayroll.DataBind();
    }

	 private void GetEditData()
    {
        if (Id != Guid.Empty)
        {
            DataSet ds = PhoenixPayRollIndia.PayRollTaxRateDetail(usercode, Id);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count  > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ddlPayroll.SelectedValue = dr["FLDPAYROLLTAXID"].ToString();
                txtSlabMinimum.Text = dr["FLDSLABMINIMUM"].ToString();
                txtSlabMaximum.Text = dr["FLDSLABMAXIMUM"].ToString();
                txtPercent.Text     = dr["FLDTAXPERCENT"].ToString();  
                //txtTaxAmount.Text   = dr["FLDSLABTAXAMOUNT"].ToString(); 
                //txtGrossTaxPayable.Text = dr["FLDGROSSTAXPAYABLE"].ToString();
            }
        }
    }

    public void ShowToolBar()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain = new PhoenixToolbar();
		if (Id == Guid.Empty)
        {
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        } 
        else
        {
            toolbarmain.AddButton("Update", "UPDATE", ToolBarDirection.Right);
        }
        gvTabStrip.MenuList = toolbarmain.Show();
    }

    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
    {
    	try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

			 if (IsValidReport())
             {
                 ucError.Visible = true;
                 return;
             }

            Guid payrolltaxid = new Guid(ddlPayroll.SelectedItem.Value);
            decimal slabmin = Convert.ToDecimal(txtSlabMinimum.Text);
            decimal slabmax = Convert.ToDecimal(txtSlabMaximum.Text);
            decimal taxpercent = Convert.ToDecimal(txtPercent.Text);
            decimal previousTaxSlab = Convert.ToDecimal(txtPreviousTaxSlab.Text);
            //decimal slabTabAmt = Convert.ToDecimal(txtTaxAmount.Text);
            //decimal grossTaxPayable = Convert.ToDecimal(txtGrossTaxPayable.Text);

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixPayRollIndia.PayRollTaxRateInsert(usercode, payrolltaxid, slabmin, slabmax, taxpercent, previousTaxSlab);
            }

			if (CommandName.ToUpper().Equals("UPDATE"))
            {
                PhoenixPayRollIndia.PayRollTaxRateUpdate(usercode, Id , payrolltaxid, slabmin, slabmax, taxpercent, previousTaxSlab);
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList();", true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidReport()
    {
        ucError.HeaderMessage = "Please add the following details";

        if (string.IsNullOrWhiteSpace(ddlPayroll.SelectedValue))
        {
            ucError.ErrorMessage = "Taxable year is mandatory";
        }

        if (string.IsNullOrWhiteSpace(txtSlabMinimum.Text))
        {
            ucError.ErrorMessage = "Slab minimum is mandatory";
        }

        if (string.IsNullOrWhiteSpace(txtSlabMaximum.Text))
        {
            ucError.ErrorMessage = "Slab maximum is mandatory";
        }

        if (string.IsNullOrWhiteSpace(txtPercent.Text))
        {
            ucError.ErrorMessage = "Tax Percent is mandatory";
        }

        //if (string.IsNullOrWhiteSpace(txtTaxAmount.Text))
        //{
        //    ucError.ErrorMessage = "Slab Tax Amount is mandatory";
        //}

        //if (string.IsNullOrWhiteSpace(txtGrossTaxPayable.Text))
        //{
        //    ucError.ErrorMessage = "Gross Tax Payable is mandatory";
        //}

        return ucError.IsError;
    }
}