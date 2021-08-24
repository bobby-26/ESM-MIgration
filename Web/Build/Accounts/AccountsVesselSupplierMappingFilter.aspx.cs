using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;

public partial class AccountsVesselSupplierMappingFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO");

        MenuOfficeFilterMain.AccessRights = this.ViewState;
        MenuOfficeFilterMain.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
  
            txtName.Focus();
            
            cblAddressType.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 33, 0, "SUP,SCH");
            cblAddressType.DataTextField = "FLDHARDNAME";
            cblAddressType.DataValueField = "FLDHARDCODE";
            cblAddressType.DataBind();
            
            cblProductType.DataSource = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                    Convert.ToInt32(PhoenixQuickTypeCode.GENERALPRODUCTTYPE));
            cblProductType.DataTextField = "FLDQUICKNAME";
            cblProductType.DataValueField = "FLDQUICKCODE";
            cblProductType.DataBind();

            ucStatus.HardTypeCode = Convert.ToInt32(PhoenixHardTypeCode.GENERALSTATUS).ToString();

            ucQAGrading.QuickTypeCode = Convert.ToInt32(PhoenixQuickTypeCode.QAGRADING).ToString();

        }
    }

    protected void OfficeFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        if (dce.CommandName.ToUpper().Equals("GO"))
        {
            ViewState["PAGENUMBER"] = 1;
            NameValueCollection criteria = new NameValueCollection();
            StringBuilder straddresstype = new StringBuilder();
            StringBuilder strproducttype = new StringBuilder();
            foreach (ListItem item in cblAddressType.Items)
            {
                if (item.Selected == true)
                {
                    straddresstype.Append(item.Value.ToString());
                    straddresstype.Append(",");
                }
            }
            if (straddresstype.Length > 1)
            {
                straddresstype.Remove(straddresstype.Length - 1, 1);
            }
            foreach (ListItem item in cblProductType.Items)
            {
                if (item.Selected == true)
                {
                    strproducttype.Append(item.Value.ToString());
                    strproducttype.Append(",");
                }
            }
            if (strproducttype.Length > 1)
            {
                strproducttype.Remove(strproducttype.Length - 1, 1);
            }
            criteria.Clear();

            criteria.Add("txtcode", txtcode.Text);
            criteria.Add("txtName", txtName.Text);
            criteria.Add("txtPhone1", txtPhone1.Text);
            criteria.Add("txtCity", txtCity.Text);
            // criteria.Add("ucState", );
            criteria.Add("ucCountry", ucCountry.SelectedCountry);
            criteria.Add("txtPostalCode", txtPostalCode.Text);
            criteria.Add("txtEMail1", txtEMail.Text);
            criteria.Add("addresstype", straddresstype.ToString());
            criteria.Add("producttype", strproducttype.ToString());
            criteria.Add("status", ucStatus.SelectedHard);
            criteria.Add("qagrading", ucQAGrading.SelectedQuick);
            //criteria.Add("pagenumber", ViewState["PAGENUMBER"].ToString());            
            Filter.CurrentSupplierMappingFilter = criteria;            
        }

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }
}
