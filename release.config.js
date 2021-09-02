module.exports = {
  // verifyConditions: [
  //     // Verifies a NUGET_TOKEN environment variable has been provided
  //     () => {
  //         if (!process.env.NUGET_TOKEN) {
  //             throw new SemanticReleaseErrorc(
  //                 'The environment variable NUGET_TOKEN is required.',
  //                 'ENOAPMTOKEN',
  //             )
  //         }
  //     },
  //     // Verifies the conditions for the plugins used below
  //     // For example verifying a GITHUB_TOKEN environment variable has been provided
  //     '@semantic-release/changelog',
  //     '@semantic-release/git',
  //     '@semantic-release/github',
  // ],
  plugins: [
    "@semantic-release/commit-analyzer",
    "@semantic-release/release-notes-generator",
    "@semantic-release/changelog",
    "@semantic-release/git",
    "@semantic-release/github",
  ],
};
