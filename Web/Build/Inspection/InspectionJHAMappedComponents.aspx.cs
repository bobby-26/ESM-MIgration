using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class InspectionJHAMappedComponents : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["JHAID"] = "";

            ViewState["JHAYN"] = "0";

            if (Request.QueryString["JHAID"] != null && Request.QueryString["JHAID"].ToString() != "")
                ViewState["JHAID"] = Request.QueryString["JHAID"].ToString();

            if (Request.QueryString["JHAYN"] != null && Request.QueryString["JHAYN"].ToString() != "")
                ViewState["JHAYN"] = Request.QueryString["JHAYN"].ToString();

            if (ViewState["JHAYN"].ToString() == "1")
            {
                BindComponents();
            }
            else
            {
                BindProcessRAComponents();
            }
        }

    }

    protected void BindProcessRAComponents()
    {
        DataSet dss = PhoenixInspectionRiskAssessmentJobHazardExtn.RAComponentList(ViewState["JHAID"].ToString() == "" ? null : General.GetNullableGuid(ViewState["JHAID"].ToString()), 2);
        int number = 1;
        if (dss.Tables[0].Rows.Count > 0)
        {
            tblcomponents.Rows.Clear();
            foreach (DataRow dr in dss.Tables[0].Rows)
            {
                CheckBox cb = new CheckBox();
                cb.ID = dr["FLDCOMPONENTID"].ToString();
                cb.Text = "";
                cb.Checked = true;
                cb.Visible = false;
                cb.AutoPostBack = true;
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

    protected void BindComponents()
    {
        DataSet dss = PhoenixInspectionRiskAssessmentJobHazardExtn.RAComponentList(ViewState["JHAID"].ToString() == "" ? null : General.GetNullableGuid(ViewState["JHAID"].ToString()), 1);
        int number = 1;
        if (dss.Tables[0].Rows.Count > 0)
        {
            tblcomponents.Rows.Clear();
            foreach (DataRow dr in dss.Tables[0].Rows)
            {
                CheckBox cb = new CheckBox();
                cb.ID = dr["FLDCOMPONENTID"].ToString();
                cb.Text = "";
                cb.Checked = true;
                cb.Visible = false;
                cb.AutoPostBack = true;                
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
}