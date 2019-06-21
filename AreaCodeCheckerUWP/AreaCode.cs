using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;

namespace AreaCodeCheckerUWP
{
    class AreaCode
    {
        private string _city;
        private int _code;
        private AreaCodeStatus _status;

        public enum AreaCodeStatus { VALID, RESERVED, INVALID };

        public string City { get => this._city; }
        public int Code { get => this._code; set => this._code = value; }
        public AreaCodeStatus Status { get => _status; }

        #region constructors
        public AreaCode()
        {
            this._code = 0;
            this._city = null;
        }

        public AreaCode(string areaCode)
        {
            int x = 0;
            AreaCodeStatus stat = AreaCodeStatus.INVALID;

            if (int.TryParse(areaCode, out x))
            {
                if (isValidIntAreaCode(x, out stat))
                {
                    if (stat == AreaCodeStatus.RESERVED)
                    {
                        this._city = "Reserved for special use";
                    }
                    else if (stat == AreaCodeStatus.INVALID)
                    {
                        this._city = "Invalid area code";
                    }
                    else
                    {
                        stat = AreaCodeStatus.VALID;
                        this._city = null;
                    }
                }
            }
            else
            {
                stat = AreaCodeStatus.INVALID;
            }
            this._code = x;
            this._status = stat;
        }

        public AreaCode(int areaCode)
        {
            AreaCodeStatus stat = AreaCodeStatus.INVALID;

            if (isValidIntAreaCode(areaCode.ToString(), out stat))
            {
                if (stat == AreaCodeStatus.RESERVED)
                {
                    this._city = "Reserved for special use";
                }
                else if (stat == AreaCodeStatus.INVALID)
                {
                    this._city = "Invalid area code";
                }
                else
                {
                    stat = AreaCodeStatus.VALID;
                    this._city = null;
                }
            }
            this._status = stat;
            this._code = areaCode;
        }
        #endregion
        #region instance methods
        #region public
        /// <summary>
        /// Calls the static GetCityAsync, passing in the areaCode field value
        /// 
        /// Uses a GET request to Google.com to try and find the name of the city that corresponds to the value passed to it.        
        /// </summary>
        /// <returns>Task<string> containing the city, state, or province name to which the area code belongs.</returns>
        public async Task<string> GetCityAsync()
        {            
            return this._city = await AreaCode.GetCityAsync(this._code.ToString());
        }
        #endregion
        #endregion
        #region static methods
        #region public
        /// <summary>
        /// Uses a GET request to Google.com to try and find the name of the city that corresponds to the area code passed in.
        /// </summary>
        /// <param name="areaCode">A string representing an area code; must be between 200 and 999, inclusive.</param>
        /// <returns>Task<string> containing the city, state, or province name to which the area code belongs.</returns>
        public static async Task<string> GetCityAsync(string areaCode)
        {
            try
            {
                return (await getCityAsync(areaCode));
            }
            catch (ArgumentException ex)
            {
                return "Invalid argument; " + ex.Message;
                throw ex;
            }
        }

        /// <summary>
        /// Uses a GET request to Google.com to try and find the name of the city that corresponds to the area code passed in.
        /// </summary>
        /// <param name="areaCode">An integer representing an area code; must be between 200 and 999, inclusive.</param>
        /// <returns>Task<string> containing the city, state, or province name to which the area code belongs.</returns>
        public static async Task<string> GetCityAsync(int areaCode)
        {
            return (await getCityAsync(areaCode.ToString()));
        }
        #endregion
        #region private
        private static bool isValidIntAreaCode(string area, out AreaCodeStatus status)
        {
            int val = 0;
            if (int.TryParse(area, out val))
            {
                // must be 3 digits, greater than 200
                if (val > 199 && val < 1000)
                {
                    status = AreaCodeStatus.VALID;
                    if ((val - 11) % 100 == 0)
                    {
                        status = AreaCodeStatus.RESERVED;
                    }
                    return true;
                }
            }
            status = AreaCodeStatus.INVALID;
            return false;
        } // isValidIntAreaCode(string, out AreaCodeStatus)

        private static bool isValidIntAreaCode(int area, out AreaCodeStatus status)
        {
            // must be 3 digits, greater than 200
            if (area > 199 && area < 1000)
            {
                status = AreaCodeStatus.VALID;
                if ((area - 11) % 100 == 0)
                {
                    status = AreaCodeStatus.RESERVED;
                }
                return true;
            }
            status = AreaCodeStatus.INVALID;
            return false;
        }// isValidIntAreaCode(string, out AreaCodeStatus)

        private static async Task<string> getCityAsync(string areaCode)
        {
            AreaCodeStatus stat = AreaCodeStatus.INVALID;

            string body = null; // full body fo the HTML response
            string div = null; // the div containing the first digit of the area code
            string code = null; // name of the city for the area code
            int val = 0; // integer value for search string creation

            if (int.TryParse(areaCode, out val)) // first make sure it's an integer
            {
                if (isValidIntAreaCode(val, out stat)) // make sure it's valid, get a AreaCodeStatus code
                {

                    if (stat == AreaCodeStatus.RESERVED)
                    {
                        return "Reserved for special use"; // 911, 411, etc
                    }
                    else
                    {
                        string url = "https://www.allareacodes.com/";                        
                        using (HttpClient client = new HttpClient())
                        {
                            try
                            {
                                HttpResponseMessage response = await client.GetAsync(url);                                
                                response.EnsureSuccessStatusCode();
                                body = await response.Content.ReadAsStringAsync();
                            }
                            catch (HttpRequestException ex)
                            {                                
                                throw ex;
                            }
                        }
                        if (body != null) // have to parse the response
                        {
                            int hundred = (int)Math.Floor((double)val / 100);
                            try
                            {
                                string search = "<div id=\"codes_" + hundred.ToString() + "\""; // find the first digit's div
                                string subSearch = "<a href=\"/" + areaCode + "\">"; // every area code in the results will have an anchor like '<a href="/222">'
                                string anchor = "<b>" + areaCode + "</b>"; // each area code is bolded before the city name                                

                                if (body.Contains(search)) // make sure we have a response containing what we need
                                {
                                    // first get the right div
                                    div = body.Substring(
                                        // index of the opening div tag for the div of the first digit of the area code
                                        (body.IndexOf(search) + search.Length),

                                        // index of the closing div tag; from the substring starting with the opening div tag
                                        (body.Substring(body.IndexOf(search) + search.Length).IndexOf("</div>")));

                                    // now need to find the area code to make sure it's valid:
                                    if (div.Contains(subSearch))
                                    {
                                        code = div.Substring( // same exact logic as above, plus stripping " - " 
                                            div.IndexOf(anchor) + anchor.Length,
                                            div.Substring(div.IndexOf(anchor) + anchor.Length).IndexOf("</a>")).TrimStart(new char[] { ' ', '-' });
                                    }
                                    //code = div.Substring(0, div.IndexOf("</div>"));
                                    
                                    if (code != null)
                                    {
                                        return code;
                                    }
                                    else
                                    {
                                        return "Not in use";
                                    }
                                }
                                else
                                {
                                    return "Not in use";
                                }
                            }
                            catch (ArgumentOutOfRangeException ex)
                            {
                                throw ex;
                            }
                        }
                    }
                }
                return "Invalid area code";
            }
            throw new ArgumentException("Integer valid required.");
        }// getCity
        #endregion
        #endregion
    }
}
