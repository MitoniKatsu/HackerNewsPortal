import { NewsStory } from './news-story.model';

export interface RankedNewsStory extends NewsStory {
  searchRanking: number;
}
