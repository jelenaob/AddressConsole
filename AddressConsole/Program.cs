using AddressConsole;
using AddressConsole.FindAdrTest;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;
using System.Web;

//string line1 = "144 Anerley Rd";
//string postcode = "SE20 8DL";
string line1 = "144 Anerley Rd";
string postcode = string.Empty;
string country = string.Empty;
string filters = $"Postcode:{postcode}";

string text = "";
//if (!string.IsNullOrEmpty(line1))
//{
//    text += line1;
//}
if (!string.IsNullOrEmpty(postcode))
{
    text = postcode;
}
if (!string.IsNullOrEmpty(line1))
{
    text += $" {line1}";
}


AddressResponse addressResponse = getAddressReponse(text, filters);
List<AddressLookup> addressesLookupList = new List<AddressLookup>();

for (int i = 0; i < addressResponse?.Items.Count; i++)
{
    if (string.Equals(addressResponse.Items[i].Type, "Address"))
    {
        AddressLookup addressLookup = createLookupAddress(addressResponse.Items[i]);
        if (addressLookup != null)
        {
            addressesLookupList.Add(addressLookup);
        }
    }
    else
    {
        AddressResponse? adrResp = getAddressById(addressResponse.Items[i].Id, filters);
        int j = 0;
        foreach (Item item in adrResp!.Items)
        {
            int index = addressResponse.Items.FindIndex(adr => string.Equals(adr.Id, item.Id));
            if (index != -1)
            {
                continue;
            }
            else
            {
                if (addressResponse.Items.Count > i + j + 1)
                {
                    addressResponse.Items.Insert(i + j + 1, item);
                }
                else
                {
                    addressResponse.Items.Add(item);
                }
            }
            j++;
        }
    }
    if (addressesLookupList.Count == 20)
    {
        break;
    }

}

if (addressesLookupList.Count == 0 && !string.IsNullOrEmpty(postcode))
{
    string strTest = string.Empty;
    addressResponse = getAddressReponse(postcode, strTest);
    for (int i = 0; i < addressResponse?.Items.Count; i++)
    {
        if (string.Equals(addressResponse.Items[i].Type, "Address"))
        {
            AddressLookup addressLookup = createLookupAddress(addressResponse.Items[i]);
            if (addressLookup != null)
            {
                addressesLookupList.Add(addressLookup);
            }
        }
        else
        {
            AddressResponse? adrResp = getAddressById(addressResponse.Items[i].Id, filters);
            int j = 0;
            foreach (Item item in adrResp!.Items)
            {
                int index = addressResponse.Items.FindIndex(adr => string.Equals(adr.Id, item.Id));
                if (index != -1)
                {
                    continue;
                }
                else
                {
                    if (addressResponse.Items.Count > i + j + 1)
                    {
                        addressResponse.Items.Insert(i + j + 1, item);
                    }
                    else
                    {
                        addressResponse.Items.Add(item);
                    }
                }
                j++;
            }
        }
        if (addressesLookupList.Count == 20)
        {
            break;
        }

    }
}

foreach (var address in addressesLookupList)
{
    Console.WriteLine(addressesLookupList.IndexOf(address) + ": " + address.OutputAddress);
}

AddressLookup createLookupAddress(Item item)
{
    string descriptionLower = item.Description.ToLower();
    string postcodeLower = postcode.ToLower();
    string line1Lower = line1.ToLower();
    if (descriptionLower.Contains(postcodeLower))
    {
        AddressLookup addressLookup = new AddressLookup();
        addressLookup.Code = "MemberId" + item.Id;
        addressLookup.MemberId = "MemberId";
        addressLookup.Line1 = line1;
        addressLookup.Postcode = postcode;
        addressLookup.Country = country;
        addressLookup.AddressId = item.Id;
        addressLookup.OutputAddress = $"{item.Text} {item.Description}";
        addressLookup.SelectedFlag = false;
        return addressLookup;
    }

    return null;

}

AddressResponse? getAddressReponse(string text, string filters)
{
    string apiKey = "GG49-HM68-KK29-PU96";
    bool isMiddleware = true;
    int limit = 20;

    var url = "https://api.addressy.com/Capture/Interactive/Find/v1.10/json3.ws?";
    url += "&Key=" + apiKey;
    url += "&Text=" + text;
    url += "&IsMiddleware=" + isMiddleware.ToString();
    url += "&Limit=" + limit.ToString();
    url += "&Filters=" + filters;

    var client = new HttpClient();
    var response = client.GetAsync(url).GetAwaiter().GetResult();

    AddressResponse? addressResponse = JsonConvert.DeserializeObject<AddressResponse>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
    return addressResponse;
}

AddressResponse getAddressById(string text, string filters)
{
    string apiKey = "GG49-HM68-KK29-PU96";
    bool isMiddleware = true;
    int limit = 20;

    var url = "https://api.addressy.com/Capture/Interactive/Find/v1.10/json3.ws?";
    url += "&Key=" + apiKey;
    url += "&IsMiddleware=" + isMiddleware;
    url += "&Limit=" + limit;
    url += "&Filters=" + filters;
    url += "&container=" + text;

    var client = new HttpClient();
    var request = new HttpRequestMessage(HttpMethod.Get, url);

    var response = client.GetAsync(url).GetAwaiter().GetResult();

    AddressResponse? addressResponse = JsonConvert.DeserializeObject<AddressResponse>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
    return addressResponse;
}
