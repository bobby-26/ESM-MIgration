using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewOffshore;
using Telerik.Web.UI;

public partial class CrewOffshore_CrewOffshoreArticlesAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);


        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Save", "SAVE", ToolBarDirection.Right);
        CrewArticelGeneral.AccessRights = this.ViewState;
        CrewArticelGeneral.MenuList = toolbarsub.Show();

        if (!IsPostBack)
        {

            chkvessel.DataSource = PhoenixRegistersVessel.ListVessel();
            chkvessel.DataTextField = "FLDVESSELNAME";
            chkvessel.DataValueField = "FLDVESSELID";
            chkvessel.DataBind();

            if (Request.QueryString["articleid"] != null && Request.QueryString["articleid"].ToString() != "")
            {
                lblnewarticleid.Text = Request.QueryString["articleid"].ToString();
                BindData();
            }
        }
        PhoenixToolbar toolbar = new PhoenixToolbar();

        if (ViewState["DTKEY"] != null && ViewState["DTKEY"].ToString() != "")
        {
            toolbar.AddImageLink("javascript:parent.openNewWindow('NAFA','','"+Session["sitepath"]+"/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKEY"].ToString() + "&mod="
                                  + PhoenixModule.OFFSHORE + "'); return false;", "Attachment", "", "ATTACHMENT", ToolBarDirection.Right);

        }
        toolbar.AddButton("Back", "ARTICLE", ToolBarDirection.Right);
        //toolbar.AddButton("Attachment", "ATTACHMENT");

        MenuCrewArticel.AccessRights = this.ViewState;
        MenuCrewArticel.MenuList = toolbar.Show();
    }
    public void BindData()
    {
        try
        {
            DataTable dt = PhoenixCrewOffshoreArticles.Artcelsedit(int.Parse(lblnewarticleid.Text));

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                ucArticalType.SelectedQuick = dr["FLDQUICKCODE"].ToString();

                foreach (RadListBoxItem li in chkvessel.Items)
                {
                    string[] vlist = dr["FLDVESSELLIST"].ToString().Split(',');
                    foreach (string s in vlist)
                    {
                        if (li.Value.Equals(s))
                        {
                            li.Checked = true;
                        }
                    }
                }
                txtupdateddate.Text = dr["FLDUPDATEON"].ToString();
                txtRemarks.Text = dr["FLDREMARKS"].ToString();
                lblnewarticleid.Text = dr["FLDARTICLEID"].ToString();
                ViewState["DTKEY"] = dr["FLDDTKEY"].ToString();

                //PhoenixToolbar toolbar = new PhoenixToolbar();
                //toolbar.AddButton("Back", "ARTICLE",ToolBarDirection.Right);


            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }


    protected void MenuCrewArticel_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("ARTICLE"))
            {
                Response.Redirect("../CrewOffshore/CrewOffshoreArticles.aspx", true);
            }

            //if (CommandName.ToUpper().Equals("ATTACHMENT"))
            //{
            //    PhoenixToolbar toolbar = new PhoenixToolbar();
            //    toolbar.AddButton("Back", "ARTICLE",ToolBarDirection.Right);
            //    if (ViewState["DTKEY"] != null && ViewState["DTKEY"].ToString() != "")
            //    {
            //        toolbar.AddImageLink("javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKEY"].ToString() + "&mod="
            //                                + PhoenixModule.OFFSHORE + "'); return false;", "Attachment", "", "ATTACHMENT");
            //    }
            //    MenuCrewArticel.MenuList = toolbar.Show();
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void CrewArticelGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string vesselList = "";
                string UservesselList = "";
                foreach (RadListBoxItem li in chkvessel.Items)
                {
                    if (li.Checked)
                    {
                        vesselList += li.Value + ",";
                    }
                }

                if (vesselList != "")
                {
                    UservesselList = "," + vesselList;
                }
                if (IsValidCrewCompanyExperience())
                {
                    
                    DateTime? updatedate;
                    updatedate = General.GetNullableDateTime(txtupdateddate.Text);
                    Guid? dtkey = null;
                    int? newarticleid = null;
                    if (Request.QueryString["articleid"] != null && Request.QueryString["articleid"].ToString() != "")
                    {
                        //ViewState["ARTICLEID"] = lblupdateddate.Text;
                        lblnewarticleid.Text = Request.QueryString["articleid"].ToString();
                    }
                    PhoenixCrewOffshoreArticles.Insertarticel(General.GetNullableInteger(lblnewarticleid.Text != null ? lblnewarticleid.Text : null)
                                                            , General.GetNullableInteger(ucArticalType.SelectedQuick)
                                                            , UservesselList
                                                            , txtRemarks.Text
                                                            , updatedate
                                                            , ref dtkey
                                                            , ref newarticleid);
                    ViewState["DTKEY"] = dtkey.ToString();
                    lblnewarticleid.Text = Convert.ToString(newarticleid);
                    ucStatus.Text = "Crew article updated";
                    BindData();
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private bool IsValidCrewCompanyExperience()
    {
        DateTime resultDate;
        Int32 resultInt;

        ucError.HeaderMessage = "Please provide the following required information";

        if (!DateTime.TryParse(txtupdateddate.Text, out resultDate))
        {
            ucError.ErrorMessage = "Updated Date is required.";
        }
        else
        if (DateTime.Parse(txtupdateddate.Text) > DateTime.Now)
        {
            ucError.ErrorMessage = "Updated Date must be less than current date";
        }

        if ((!Int32.TryParse(ucArticalType.SelectedQuick, out resultInt)) || ucArticalType.SelectedQuick == "0")
        {
            ucError.ErrorMessage = "Update Done is required.";
        }

        if (chkvessel.CheckedItems.Count <=0)
        {
            ucError.ErrorMessage = "Vessel is required.";
        }

        return (!ucError.IsError);
    }
}
