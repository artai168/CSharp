using BlazorApp.Interfaces.Data;

namespace BlazorApp.Address_Analysis.Interface
{
    interface IAddressMatchingEngine
    {
        //Set _RAW_string to the engine
        void Set_Raw();
        bool With_AreaInfo(); 
        bool With_StreetInfo();
        bool With_DistrictInfo();
        bool With_BuildingInfo();
        bool With_AddressInfo();
        IFullAddress<string> GetFullAddress();
        void Set_AreaInfo();
        void Set_StreetInfo();
        void Set_DistrictInfo();
        void Set_AddressInfo();
    }
}
