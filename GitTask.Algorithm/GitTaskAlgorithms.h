#ifdef GITTASKALGORITHM_EXPORTS
#define GITTASKALGORITHM_API __declspec(dllexport) 
#else
#define GITTASKALGORITHM_API __declspec(dllimport) 
#endif

#include <string>
#include <forward_list>
#include <vector>
#include <unordered_map>

using namespace std;

namespace GitTask
{
	class UserResolver
	{
		string** names;
		string** emails;
		unsigned resultsLength;
		vector<unsigned> namesLengths;
		vector<unsigned> emailsLengths;
		unordered_map<string, vector<pair<string, long>>> emailBags;

	public:
		GITTASKALGORITHM_API void addCommitInfo(string name, string email, long timeAsTicks);
		GITTASKALGORITHM_API void calculate();
		GITTASKALGORITHM_API unsigned getResultsLength() const;
		GITTASKALGORITHM_API string** getNames() const; // returns collection of users names sets, where first name is the main
		GITTASKALGORITHM_API string** getEmails() const; // returns collection of users emails sets, where first email is the main
		GITTASKALGORITHM_API void cleanMemory() const;
	};


	class UserSet
	{
	public:
		string mainName;
		string mainEmail;

		forward_list<string> names;
		forward_list<string> emails;
	};
}
