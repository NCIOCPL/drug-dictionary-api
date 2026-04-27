namespace NCI.OCPL.Api.DrugDictionary.Tests
{

    /// <summary>
    /// Constants used by drugs controller tests.
    /// </summary>
    public partial class DrugsControllerTests
    {
        const int DEFAULT_SEARCH_SIZE = 100;

        const int DEFAULT_SEARCH_FROM = 0;

        static readonly DrugResourceType[] DEFAULT_DRUG_RESOURCE_TYPE_LIST = { DrugResourceType.DrugTerm, DrugResourceType.DrugAlias };

        static readonly TermNameType[] DEFAULT_INCLUDED_TERM_TYPE_LIST = { TermNameType.CodeName, TermNameType.ObsoleteName, TermNameType.Abbreviation,
                            TermNameType.INDCode, TermNameType.NSCNumber, TermNameType.ForeignBrandName, TermNameType.Synonym,
                            TermNameType.CASRegistryName, TermNameType.Subtype, TermNameType.Spanish, TermNameType.LexicalVariant,
                            TermNameType.ChemicalStructureName, TermNameType.CommonUsage, TermNameType.Acronym, TermNameType.USBrandName,
                            TermNameType.Broader, TermNameType.RelatedString, TermNameType.PreferredName };

        static readonly TermNameType[] DEFAULT_EXCLUDED_TERM_TYPE_LIST = { };
    }
}