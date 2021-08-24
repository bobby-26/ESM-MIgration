using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class InspectionJHAPPEWithIcons : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["TYPE"] = Request.QueryString["TYPE"];
                ViewState["JHAID"] = "";

                ViewState["JHAYN"] = "0";

                if (Request.QueryString["JHAID"] != null && Request.QueryString["JHAID"].ToString() != "")
                    ViewState["JHAID"] = Request.QueryString["JHAID"].ToString();

                if (Request.QueryString["JHAYN"] != null && Request.QueryString["JHAYN"].ToString() != "")
                    ViewState["JHAYN"] = Request.QueryString["JHAYN"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        if (ViewState["JHAYN"].ToString() == "1")
        {

            DataTable DT = PhoenixInspectionRiskAssessmentHazardExtn.ListJHAPPEWithIcons(new Guid(ViewState["JHAID"].ToString()));

            gvRAMiscellaneous.DataSource = DT;
        }
        else
        {
            DataTable DT = PhoenixInspectionRiskAssessmentHazardExtn.ListProcessRAPPEWithIcons(new Guid(ViewState["JHAID"].ToString()));

            gvRAMiscellaneous.DataSource = DT;
        }

    }
    protected void gvRAMiscellaneous_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            Image img = (Image)e.Item.FindControl("imgPhoto");
            img.Attributes.Add("src", drv["FLDIMAGE"].ToString());

            Label lblMiscellaneousId = (Label)e.Item.FindControl("lblMiscellaneousId");
            HtmlTable tblForms = (HtmlTable)e.Item.FindControl("tblForms");
            if (lblMiscellaneousId != null)
            {
                DataSet dss = PhoenixInspectionRiskAssessmentMiscellaneousExtn.PPEEPSSList(lblMiscellaneousId.Text == null ? null : General.GetNullableInteger(lblMiscellaneousId.Text));
                int number = 1;
                if (dss.Tables[0].Rows.Count > 0)
                {
                    tblForms.Rows.Clear();
                    foreach (DataRow dr in dss.Tables[0].Rows)
                    {
                        HyperLink hl = new HyperLink();
                        hl.Text = dr["FLDNAME"].ToString();
                        hl.ID = "hlink" + number.ToString();
                        hl.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");

                        string type = dr["FLDTYPE"].ToString();

                        if (type == "5")
                        {
                            hl.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&FORMID=" + dr["FLDEPSSID"].ToString() + "&FORMREVISIONID=" + dr["FLDFORMREVISIONID"].ToString() + "',false,700,700);return false;");
                        }
                        else if (type == "6")
                        {
                           hl.Target = "_blank";
                           hl.NavigateUrl = "../Common/download.aspx?formid=" + dr["FLDEPSSID"].ToString();
                        }

                        HtmlTableRow tr = new HtmlTableRow();
                        HtmlTableCell tc = new HtmlTableCell();
                        tr.Cells.Add(tc);
                        tc = new HtmlTableCell();
                        tc.Controls.Add(hl);
                        tr.Cells.Add(tc);
                        tblForms.Rows.Add(tr);
                        tc = new HtmlTableCell();
                        tc.InnerHtml = "<br/>";
                        tr.Cells.Add(tc);
                        tblForms.Rows.Add(tr);
                        number = number + 1;
                    }
                }
            }
        }        
    }

    protected void gvRAMiscellaneous_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}